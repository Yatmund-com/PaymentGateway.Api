using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PaymentGateway.Api.Contract;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Integration.Tests
{
    [TestClass]
    public class PaymentGatewayApiShould
    {
        private const string SuccessCardNo = "4485 7442 9591 0532";
        private const string FailedCardNo = "5298 5127 5869 2572";
        private const string ProcessUrl = "/payment/process";
        private const string GetDetailsUrl = "payment/details";
        private Fixture _fixture;
        private HttpClient _httpClient;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
            _httpClient = TestServerSetup.CreateHttpClient();
        }

        [TestMethod]
        public async Task Return_Successful_ProcessResponse()
        {
            // Arrange
            var processRequest = _fixture.Build<ProcessRequest>()
                                    .With(x => x.CardNo, SuccessCardNo)
                                    .Create();

            // Act
            var processResponse = await _httpClient.PostAsJsonAsync(ProcessUrl, processRequest);


            // Assert
            processResponse.EnsureSuccessStatusCode();
            var processResponseContent = await processResponse.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<ProcessResponse>(processResponseContent);
            Assert.IsTrue(deserializedResponse.Success);
        }

        [TestMethod]
        public async Task Return_Failed_ProcessResponse()
        {
            // Arrange
            var processRequest = _fixture.Build<ProcessRequest>()
                                    .With(x => x.CardNo, FailedCardNo)
                                    .Create();

            // Act
            var processResponse = await _httpClient.PostAsJsonAsync(ProcessUrl, processRequest);


            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, processResponse.StatusCode);
            var processResponseContent = await processResponse.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<ProcessResponse>(processResponseContent);
            Assert.IsFalse(deserializedResponse.Success);
        }

        [TestMethod]
        public async Task Return_Successful_DetailsResponse_ByMerchantId()
        {
            // Act
            var processResponse = await _httpClient.GetAsync(GetDetailsUrl + "?merchantId=merchantId1");

            // Assert
            processResponse.EnsureSuccessStatusCode();
            var processResponseContent = await processResponse.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<DetailsResponse>(processResponseContent);
            Assert.IsTrue(deserializedResponse.Success);
        }
        [TestMethod]
        public async Task Return_Successful_DetailsResponse_ByPaymentId()
        {
            // Act
            var processResponse = await _httpClient.GetAsync(GetDetailsUrl + "?paymentId=d54471eb-db6a-4738-b014-11ca39818889");

            // Assert
            processResponse.EnsureSuccessStatusCode();
            var processResponseContent = await processResponse.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<DetailsResponse>(processResponseContent);
            Assert.IsTrue(deserializedResponse.Success);
        }
    }
}
