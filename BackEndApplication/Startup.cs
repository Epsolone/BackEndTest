using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using BackEndApplication.Interfaces;
using BackEndApplication.Models;
using BackEndApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackEndApplication
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

            var typesWithMappings = new List<Type>()
            {
                typeof(AutomapperProfile)
            };

            var assembliesWithMappings = typesWithMappings
                .Select(t => t.Assembly)
                .Concat(new List<Assembly>() { Assembly.GetExecutingAssembly() });

            services.AddAutoMapper(assembliesWithMappings);

            services.AddTransient<ICachedDataService, CachedDataService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddHostedService<LoadDataHostedService>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
