using Newtonsoft.Json;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Integration
{
    public class BankIntegration : IBankIntegration
    {
        private readonly IConfigurationReader _configurationReader;
        private readonly IHttpClientFactory _httpClientFactory;
        public BankIntegration(IConfigurationReader configurationReader, IHttpClientFactory httpClientFactory)
        {
            _configurationReader = configurationReader;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<BankResponse> SendtoAcquirerAsync(BankRequest bankRequest)
        {
            var url = _configurationReader.Read(bankRequest.Bank).Url;

            var httpClient = _httpClientFactory.CreateClient();

            var result = await httpClient.PostAsJsonAsync(url, bankRequest);

            var response = JsonConvert.DeserializeObject<BankResponse>(await result.Content.ReadAsStringAsync());

            response.TransactionId = Guid.NewGuid().ToString();

            return response;
        }
    }
}