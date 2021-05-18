using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace PaymentGateway.Api.Integration.Tests
{
    public class TestServerSetup
    {
        public static HttpClient CreateHttpClient()
        {
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