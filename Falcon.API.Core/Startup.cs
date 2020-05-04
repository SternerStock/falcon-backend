namespace Falcon.API
{
    using System;
    using Falcon.MtG;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<MtGDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MtGDBContext")));

            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Falcon", new OpenApiInfo
                {
                    Version = "3.0.0",
                    Title = "Falcon",
                    Description = "Falcon Syndicate Site API",
                    TermsOfService = null,
                    Contact = new OpenApiContact() { Name = "Corey Laird", Email = "captain@falconsyndicate.net", Url = new Uri("https://www.falconsyndicate.net/") }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(
                    options => options.WithOrigins("http://localhost:8000").AllowAnyMethod()
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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