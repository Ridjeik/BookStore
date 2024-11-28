using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;
using BLL.Extensions;
using LibraryCW.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BLL.Models.AppSettings;
using LibraryCW;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins("http://localhost:5173/")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));
builder.Services.AddServices(builder.Configuration, builder.Configuration.GetConnectionString());

var applicationSettingsConfiguration = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(applicationSettingsConfiguration);
applicationSettingsConfiguration.Get<JwtSettings>();
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    NameClaimType = ClaimTypes.NameIdentifier,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               .AddJwtBearer(o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       NameClaimType = ClaimTypes.NameIdentifier,
                       ClockSkew = TimeSpan.Zero,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                   };

                   o.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                           var accessToken = context.Request.Query["access_token"];
                           if (string.IsNullOrEmpty(accessToken) == false)
                           {
                               context.Token = accessToken;
                           }
                           else
                           {
                               var authHeader = context.Request.Headers["Authorization"];
                               if (authHeader.Count > 0 && authHeader[0].StartsWith("Bearer "))
                               {
                                   context.Token = authHeader[0][7..]; // Skip "Bearer "
                               }
                               else
                               {
                                   var cookieToken = context.Request.Cookies[ApiConstants.JwtToken];
                                   if (!string.IsNullOrEmpty(cookieToken))
                                   {
                                       context.Token = cookieToken;
                                   }
                                   else
                                   {
                                       logger.LogInformation("No access token found in the request.");
                                   }
                               }
                           }

                           return Task.CompletedTask;
                       }
                   };

               });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("Employee", policy => policy.RequireClaim(ClaimTypes.Role, "Employee"));
    options.AddPolicy("Member", policy => policy.RequireClaim(ClaimTypes.Role, "Member"));
    options.AddPolicy("AdminOrEmployee", policy => policy.RequireAssertion(context =>
        context.User.HasClaim(c =>
            (c.Type == ClaimTypes.Role) &&
            (c.Value == "Admin" || c.Value == "Employee"))));

});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    // Set the comments path for the Swagger JSON and UI.
    /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);*/

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Book}/{action=Index}/{id?}");
    endpoints.MapControllers();
});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedDb.SeedData(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
