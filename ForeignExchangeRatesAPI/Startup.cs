using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using ForeignExchangeRates.Service.Services;
using ForeignExchangeRates.Service.Services.Impl;
using ForeignExchangeRates.Service.Settings;
using ForeignExchangeRates.WebAPI.Extensions;
using ForeignExchangeRates.WebAPI.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;

namespace ForeignExchangeRates.WebAPI
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
            services.AddMvc()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ExchangeRatesRequestDtoValidator>());

            services.Configure<ExchangeRatesApiSettings>(Configuration.GetSection(
                ExchangeRatesApiSettings.ExchangeRatesApi));

            services.AddTransient<IExchangeRatesService, ExchangeRatesService>();
            services.AddTransient<IExchangeRateConnector, ExchangeRateConnector>();

            services.AddSwaggerGen();

            services.ConfigurePollyPolicies();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.ConfigureExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
