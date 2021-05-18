using Microsoft.Extensions.Configuration;
using PaymentGateway.Api.Contract;

namespace PaymentGateway.Api.Configuration
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public ConfigurationReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BankConfiguration Read(string bank) => _configuration.GetSection($"BankConfig:{bank}").Get<BankConfiguration>();
    }
}
