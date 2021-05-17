using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Integration.Tests
{
    public class TestServerSetup
    {
        public static HttpClient CreateHttpClient()
        {
            var projectDir = Directory.GetCurrentDirectory();

            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile("appsettings.json");
                    conf.AddJsonFile("appsettings.Development.json");
                });
            });

            return factory.CreateClient();
        }
    }
}