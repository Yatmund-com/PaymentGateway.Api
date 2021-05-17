using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Service;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Tests
{
    [TestClass]
    public class PaymentControllerShould
    {
        private Fixture _fixture;
        private Mock<IPaymentService> _paymentServiceMock;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
            _paymentServiceMock = new Mock<IPaymentService>();
        }

        [TestMethod]
        public async Task ProcessPayment_and_ReturnCorrectResponse()
        {
            // Arrange
            var request = _fixture.Create<ProcessRequest>();

            var httpContext = new DefaultHttpContext();

            var controller = new PaymentController(NullLogger<PaymentController>.Instance, _paymentServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            var paymentId = Guid.NewGuid().ToString();

            _paymentServiceMock.Setup(x => x.ProcessPaymentAsync(request))
                               .ReturnsAsync(new ProcessResponse { Success = true, PaymentId = paymentId });

            // Act
            var result = await controller.ProcessAsync(request);
            var response = result.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.AreEqual(paymentId, response.PaymentId);
        }
    }
}
