using AuthServer.Core.Configuration;
using AuthServer.Core.Domain;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using AuthServer.Data.Concrete.EntityFrameworkCore.Contexts;
using AuthServer.Data.Concrete.EntityFrameworkCore.Repositories;
using AuthServer.Data.Concrete.EntityFrameworkCore.UnitOfWorks;
using AuthServer.Service.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServer.API.Middlewares
{
    public static class CustomServicesConfiguration
    {
        public static void AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CustomTokenSetting>(configuration.GetSection(nameof(CustomTokenSetting)));
            services.Configure<List<Client>>(configuration.GetSection(nameof(Client)));
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

        public static void AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var customTokenSettings = configuration.GetSection(nameof(CustomTokenSetting)).Get<CustomTokenSetting>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
            {
                configureOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = customTokenSettings.Issuer,
                    ValidAudience = customTokenSettings.Audience[0],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(customTokenSettings.SecurityKey)),

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero //Sunucular arasındaki oluşabilecek kısa zaman farkı

                    /* Farklı ülkelerde ki sunuculardaki zaman farkından dolayı Aynı yerel saate baksana aralarında belki de 1-2 saniye yada 2-3 dakika
                     * gibi farklar olabilir. Bundan dolayı JWT verilen zamana 5 dakika ekler. Biz ClockSkew'e zero diyerek bu eklenen 5 dakikayı siliyoruz. (Gerek duymadıgımız için)
                     */
                };
            });
        }
    }
}
