using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PaymentGateway.Api.Configuration;
using PaymentGateway.Api.Integration;
using PaymentGateway.Api.Service;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace PaymentGateway.Api
{
    public class Startup
    {
        public const string ApiName = "PaymentGateway.Api";
        private const string ApiDescription = "Checkout.com technical test";
        private const string ApiVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IPaymentService, PaymentService>();
            RegisterDocumentationGenerator(services);
            services.AddHttpClient(Options.DefaultName)
                   .ConfigurePrimaryHttpMessageHandler(h =>
                   {
                       var handler = new HttpClientHandler();
                       handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli;
                       return handler;
                   });
            services.AddScoped<IConfigurationReader, ConfigurationReader>();
            services.AddScoped<IBankIntegration, BankIntegration>(s => new BankIntegration(s.GetService<IConfigurationReader>(), s.GetService<IHttpClientFactory>()));
        }

        private void RegisterDocumentationGenerator(IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            // more info can be found https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion,
                    new OpenApiInfo
                    {
                        Version = ApiVersion,
                        Title = ApiName,
                        Description = ApiDescription
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(s => s.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentGateway.Api v1"));
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
