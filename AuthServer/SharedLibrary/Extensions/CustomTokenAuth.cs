using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Settings;
using System;
using System.Text;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
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
