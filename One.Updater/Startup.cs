using System;
using System.Net.Http;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using One.Core;
using One.Core.Interfaces;
using One.DB;
using One.Infrastructure;

namespace One.Updater
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
            services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString));
            services.AddHangfireServer();
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
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
            
            
            RecurringJob.AddOrUpdate<WeatherService>(
                "weather-update",
                s => s.UpdateDB(),
                Cron.Daily);
        }
    }
}