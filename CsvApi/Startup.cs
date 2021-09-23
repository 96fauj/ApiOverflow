using CsvApp.Business;
using CsvApp.Business.Models;
using CsvApp.Business.Parsers;
using CsvApp.Business.Repository;
using EnergyDataLayer.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddDbContext<EnergyDbContext>(
                opt => opt.UseInMemoryDatabase("EnergyDb"));

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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
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

            SeedEfDatabase(app);
        }

        private static void SeedEfDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EnergyDbContext>();
                var parser = new AccountRowParser();
                var result = parser.ParseCsvFromResourceFile("CsvApp.Business.Repository.Test_Accounts.csv");
                var accounts = parser.CsvEntityToAccounts(result.GoodRows.Values);
                context.SeedAccounts(accounts);
            }
        }
    }
}