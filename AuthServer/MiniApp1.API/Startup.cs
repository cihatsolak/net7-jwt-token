using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniApp1.API.ClaimsRequirements;
using SharedLibrary.Extensions;
using System.Security.Claims;

namespace MiniApp1.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddAuthenticationConfiguration(Configuration);

            //Requirement Handlers
            services.AddSingleton<IAuthorizationHandler, AgeRequirementHandler>();

            //Claimsleri kullanarak Policy şablonları oluşturma
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TurkishPeople", policy => //Claims-based
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Country, "Türkiye");
                });

                options.AddPolicy("BritishPeople", policy => //Claims-based
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Country, "England", "Scotland", "Wales", "NorthernIreland");
                });

                options.AddPolicy("TurkishAndOver18YearsOld", policy => //Claims and Requirement based
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Country, "Türkiye");
                    policy.Requirements.Add(new AgeRequirement(18));
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
