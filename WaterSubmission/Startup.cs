using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WaterSubmission.Data;
using WaterSubmission.Services.AccountingControlService;
using WaterSubmission.Services.PricingService;
using WaterSubmission.Services.StatusControlService;

namespace WaterSubmission
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            CurrentEnvironment = env;
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{CurrentEnvironment.EnvironmentName}.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IAccountControlKubeMQSettings mqSettings = new AccountControlKubeMQSettings();
            Configuration.GetSection("AccountControlKubeMQSettings").Bind(mqSettings);
            services.AddSingleton(mqSettings);

            services.AddControllers();

            services.AddScoped<IAccountingControlService, AccountingControlService>();
            services.AddScoped<IStatusControlService, MockStatusControlService>();
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<HttpClient, HttpClient>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Water Submission API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Water Submission API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
