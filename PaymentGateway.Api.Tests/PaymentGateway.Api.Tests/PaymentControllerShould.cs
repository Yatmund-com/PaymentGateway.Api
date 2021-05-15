using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Controllers;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Tests
{
    [TestClass]
    public class PaymentControllerShould
    {
        private Fixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
        }
        [TestMethod]
        public async Task ProcessPayment_and_ReturnCorrectResponse()
        {
            // Arrange
            var request = _fixture.Create<ProcessRequest>();

            var httpContext = new DefaultHttpContext();

            var controller = new PaymentController(NullLogger<PaymentController>.Instance)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var result = await controller.ProcessAsync(request);

            // Assert
            Assert.IsNotNull(result);

        }
    }
}
