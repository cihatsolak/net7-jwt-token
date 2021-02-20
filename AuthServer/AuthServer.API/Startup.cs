using AuthServer.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AuthServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Bu yöntem çalýþma zamaný tarafýndan çaðrýlýr. Kapsayýcýya hizmet eklemek için bu yöntemi kullanýn.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSettingsConfiguration(Configuration);
            services.AddServicesConfiguration(Configuration);
            services.AddAuthenticationConfiguration(Configuration);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthServer.API", Version = "v1" });
            });
        }

        // Bu yöntem çalýþma zamaný tarafýndan çaðrýlýr. HTTP istek ardýþýk düzenini yapýlandýrmak için bu yöntemi kullanýn.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthServer.API v1"));
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
