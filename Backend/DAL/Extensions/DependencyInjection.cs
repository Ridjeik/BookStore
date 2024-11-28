using DAL.EF;
using DAL.Repositories.Implemantations;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, string connectionString)
    {
        services
            .AddDbContext<DbContext, LibraryDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Scoped)
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Lockout.MaxFailedAccessAttempts = 6;
            })
          .AddEntityFrameworkStores<LibraryDbContext>()
          .AddDefaultTokenProviders();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookGenreRepository, BookGenreRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ITitleBookRepository, TitleBookRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IShelfRepository, ShelfRepository>();
        services.AddScoped<IRowRepository, RowRepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<IShelfGenreRepository, ShelfGenreRepository>();
        services.AddScoped<IBookInfoRepository, BookInfoRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();

        return services;
    }
}