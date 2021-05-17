using AutoFixture;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Integration;
using PaymentGateway.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Tests
{
    [TestClass]
    public class PaymentServiceShould
    {
        private Fixture _fixture;
        private Mock<IBankIntegration> _bankIntegrationMock;
        private const string SuccessCardNo = "4485 7442 9591 0532";
        private const string FailedCardNo = "5298 5127 5869 2572";

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
            _bankIntegrationMock = new Mock<IBankIntegration>();
        }

        [TestMethod]
        public async Task Return_Successful_ProcessResponse()
        {
            // Arrange
            var processRequest = _fixture.Build<ProcessRequest>()
                                    .With(x => x.CardNo, SuccessCardNo)
                                    .Create();

            _bankIntegrationMock.Setup(x => x.SendtoAcquirerAsync(It.IsAny<BankRequest>()))
                                     .ReturnsAsync(new BankResponse { Success = true, TransactionId = Guid.NewGuid().ToString() });

            var paymentService = new PaymentService(_bankIntegrationMock.Object);

            // Act
            var response = await paymentService.ProcessPaymentAsync(processRequest);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task Return_Failed_ProcessResponse()
        {
            // Arrange
            var processRequest = _fixture.Build<ProcessRequest>()
                                    .With(x => x.CardNo, FailedCardNo)
                                    .Create();

            _bankIntegrationMock.Setup(x => x.SendtoAcquirerAsync(It.IsAny<BankRequest>()))
                                     .ReturnsAsync(new BankResponse { Success = false, TransactionId = Guid.NewGuid().ToString() });

            var paymentService = new PaymentService(_bankIntegrationMock.Object);

            // Act
            var response = await paymentService.ProcessPaymentAsync(processRequest);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
        }
    }
}
