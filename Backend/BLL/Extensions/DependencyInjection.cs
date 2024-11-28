using AutoMapper;
using BLL.Helpers;
using BLL.Models.AppSettings;
using BLL.Services.Implemantation;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {


/*            services
                .Configure<CloudinaryOptions>(options =>
                {
                    options.CloudName = configuration.GetSection("Cloudinary:CloudName").Value;
                    options.ApiKey = configuration.GetSection("Cloudinary:ApiKey").Value;
                    options.ApiSecret = configuration.GetSection("Cloudinary:ApiSecret").Value;
                });*/



            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IDataSetsService, DataSetsService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IShelfService, ShelfService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IRowService, RowService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<ITitleService, TitleService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddPersistence(configuration, connectionString);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            // Validate all mappings
            //mapperConfig.AssertConfigurationIsValid();

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
