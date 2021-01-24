using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using One.Core;
using One.DB;
using One.Core.Interfaces;
using One.Infrastructure;
using Microsoft.Extensions.Logging;

namespace One
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
            var connectionString = Configuration.GetSection("DbConnection").Get<string>();
            var apiKey = Configuration.GetSection("ApiKey").Get<string>();
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddHttpClient();
            services.AddTransient<IWeatherMapClient>(sp =>
            {
                var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new WeatherMapClientImpl(apiKey, clientFactory.CreateClient());
            });
            services.AddTransient<IWeatherRepo>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<WeatherRepo>>();
                return new WeatherRepo(connectionString, logger);
            });
            services.AddTransient<WeatherService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
