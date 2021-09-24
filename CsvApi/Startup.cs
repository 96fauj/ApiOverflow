using System.Text.Json;
using System.Text.Json.Serialization;
using CsvApp.Business.Config;
using CsvApp.Business.Interfaces;
using CsvApp.Business.Parsers;
using CsvApp.Business.Services;
using EnergyDataLayer.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CsvApi
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
            services.AddDbContext<DbContext, EnergyDbContext>(
                opt => opt.UseInMemoryDatabase("EnergyDb"));
            services.AddDbContext<EnergyDbContext>(
                opt => opt.UseInMemoryDatabase("EnergyDb"));

            services.AddTransient<IEnergyService, EfEnergyDbService>();

            services.AddControllers();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meter Reading Api V1");
                c.RoutePrefix = "swagger"; // endpoint
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // MeterRead Config 
            var settings = 
                Configuration.GetSection("MeterReadConfigSettings").Get<MeterReadConfigSettings>();
            settings.SetupConfigValues();

            SeedEfDatabase(app, Configuration);

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonSerializer.Serialize(new { error = exception.Message });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
        }

        private static void SeedEfDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EnergyDbContext>();
                var parser = new AccountRowParser();
                var fileLocation = configuration.GetSection("AccountSeedFileLocation").Value;
                var result = parser.ParseCsvFromResourceFile(fileLocation);
                var accounts = parser.CsvEntityToAccounts(result.GoodRows.Values);
                context.SeedAccounts(accounts);
            }
        }
    }
}