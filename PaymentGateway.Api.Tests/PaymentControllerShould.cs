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
using System.Net;
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
            var result = await controller.ProcessAsync(request) as OkObjectResult;
            var response = result.Value as ProcessResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(paymentId, response.PaymentId);
        }

        [TestMethod]
        public async Task ReturnBadRequest_On_FailedPayment()
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
                               .ReturnsAsync(new ProcessResponse { Success = false, PaymentId = paymentId, Message = "failedPayment" });

            // Act
            var result = await controller.ProcessAsync(request) as BadRequestObjectResult;
            var response = result.Value as ProcessResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(paymentId, response.PaymentId);
        }

        [TestMethod]
        public async Task Return_Successful_DetailsResponse_By_MerchantId()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var controller = new PaymentController(NullLogger<PaymentController>.Instance, _paymentServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            var detailsResponse = _fixture.Build<DetailsResponse>()
                                    .With(x => x.Success, true)
                                    .Create();

            _paymentServiceMock.Setup(x => x.GetDetailsAsync("merchantId1", null))
                               .ReturnsAsync(detailsResponse);

            // Act
            var result = await controller.GetDetailsAsync("merchantId1", null) as OkObjectResult;
            var response = result.Value as DetailsResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(detailsResponse.MaskedCardNo, response.MaskedCardNo);
        }

        [TestMethod]
        public async Task Return_Successful_DetailsResponse_By_PaymentId()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var controller = new PaymentController(NullLogger<PaymentController>.Instance, _paymentServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            var detailsResponse = _fixture.Build<DetailsResponse>()
                                    .With(x => x.Success, true)
                                    .Create();

            _paymentServiceMock.Setup(x => x.GetDetailsAsync(null, "paymentId1"))
                               .ReturnsAsync(detailsResponse);

            // Act
            var result = await controller.GetDetailsAsync(null, "paymentId1") as OkObjectResult;
            var response = result.Value as DetailsResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(detailsResponse.MaskedCardNo, response.MaskedCardNo);
        }

        [TestMethod]
        public async Task Return_NotFound_When_Null()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var controller = new PaymentController(NullLogger<PaymentController>.Instance, _paymentServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var result = await controller.GetDetailsAsync(null, "paymentId1") as NotFoundObjectResult;
            var response = result.Value as DetailsResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsFalse(response.Success);
        }
    }
}
