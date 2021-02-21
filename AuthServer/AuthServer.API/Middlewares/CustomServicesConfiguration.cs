using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using AuthServer.Data.Concrete.EntityFrameworkCore.Contexts;
using AuthServer.Data.Concrete.EntityFrameworkCore.Repositories;
using AuthServer.Data.Concrete.EntityFrameworkCore.UnitOfWorks;
using AuthServer.Service.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Settings;
using System.Collections.Generic;

namespace AuthServer.API.Middlewares
{
    public static class CustomServicesConfiguration
    {
        public static void AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CustomTokenSetting>(configuration.GetSection(nameof(CustomTokenSetting)));
            services.Configure<List<Client>>(configuration.GetSection("ClientSettings"));
        }

        public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"), sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly("AuthServer.Data"); //Migration işlemini hangi class library'de gerçekleştirmek istiyorum?
                });
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); //Şifre sıfırlamada default token üretmesi

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
