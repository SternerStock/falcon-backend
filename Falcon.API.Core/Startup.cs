namespace Falcon.API
{
    using System;
    using Falcon.MtG;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            string connectionString = Configuration.GetConnectionString("MtGDBContext");
            services.AddDbContext<MtGDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Falcon", new OpenApiInfo
                {
                    Version = "3.1.0",
                    Title = "Falcon",
                    Description = "Falcon Syndicate Site API",
                    TermsOfService = null,
                    Contact = new OpenApiContact() { Name = "Corey Laird", Email = "me@coreylaird.com", Url = new Uri("https://www.falconsyndicate.net/") }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(
                    options => options.WithOrigins("http://localhost:8000").AllowAnyMethod().AllowAnyHeader()
                );
            } else
            {
                app.UseCors(
                    options => options.WithOrigins("http://www.falconsyndicate.net").AllowAnyMethod().AllowAnyHeader()
                );
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Falcon/swagger.json", "Falcon Syndicate Site API");
            });
        }
    }
}