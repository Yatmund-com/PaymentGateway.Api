using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Integration;
using PaymentGateway.Api.Operation;
using PaymentGateway.Api.Service;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Tests
{
    [TestClass]
    public class PaymentServiceShould
    {
        private Fixture _fixture;
        private Mock<IBankIntegration> _bankIntegrationMock;
        private Mock<IOperations> _operationsMock;
        private const string SuccessCardNo = "4485 7442 9591 0532";
        private const string FailedCardNo = "5298 5127 5869 2572";
        private const string MerchantId = "merchantId1";
        private const string PaymentId = "paymnetId1";

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
            _bankIntegrationMock = new Mock<IBankIntegration>();
            _operationsMock = new Mock<IOperations>();
        }

        [TestMethod]
        public async Task Return_Successful_ProcessResponse()
        {
            // Arrange
            var processRequest = _fixture.Build<ProcessRequest>()
                                    .With(x => x.CardNo, SuccessCardNo)
                                    .Create();
            var bankResponse = new BankResponse { Success = true, TransactionId = Guid.NewGuid().ToString() };
            _bankIntegrationMock.Setup(x => x.SendtoAcquirerAsync(It.IsAny<BankRequest>()))
                                     .ReturnsAsync(bankResponse);

            _operationsMock.Setup(x => x.AddPaymentAsync(It.IsAny<ProcessRequest>(), bankResponse, It.IsAny<string>()))
                                             .ReturnsAsync(new ResponseBase { Success = true });

            var paymentService = new PaymentService(_bankIntegrationMock.Object, _operationsMock.Object, NullLogger<PaymentService>.Instance);

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

            var bankResponse = new BankResponse { Success = false, TransactionId = Guid.NewGuid().ToString() };

            _bankIntegrationMock.Setup(x => x.SendtoAcquirerAsync(It.IsAny<BankRequest>()))
                                     .ReturnsAsync(bankResponse);
            _operationsMock.Setup(x => x.AddPaymentAsync(It.IsAny<ProcessRequest>(), bankResponse, It.IsAny<string>()))
                                             .ReturnsAsync(new ResponseBase { Success = true });

            var paymentService = new PaymentService(_bankIntegrationMock.Object, _operationsMock.Object, NullLogger<PaymentService>.Instance);

            // Act
            var response = await paymentService.ProcessPaymentAsync(processRequest);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task Return_DetailsResponse_FromSuccessfulProcess_By_MerchantId()
        {
            // Arrange
            var paymentService = new PaymentService(_bankIntegrationMock.Object, _operationsMock.Object, NullLogger<PaymentService>.Instance);

            var paymentLog = _fixture.Build<PaymentLog>()
                                .With(x => x.Success, true)
                                .With(x => x.MerchantId, MerchantId)
                                .Create();

            _operationsMock.Setup(x => x.GetPaymentDetailsByPaymentIdOrMerchantId(null, MerchantId))
                                             .ReturnsAsync(paymentLog);

            // Act
            var response = await paymentService.GetDetailsAsync(MerchantId, null);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(paymentLog.MaskedCardNo, response.MaskedCardNo);
            Assert.AreEqual(paymentLog.Amount, response.Amount);
            Assert.AreEqual(paymentLog.Reason, response.BankMessage);
        }

        [TestMethod]
        public async Task Return_DetailsResponse_FromSuccessfulProcess_By_PaymentId()
        {
            // Arrange
            var paymentService = new PaymentService(_bankIntegrationMock.Object, _operationsMock.Object, NullLogger<PaymentService>.Instance);

            var paymentLog = _fixture.Build<PaymentLog>()
                                .With(x => x.Success, true)
                                .With(x => x.PaymentId, PaymentId)
                                .Create();

            _operationsMock.Setup(x => x.GetPaymentDetailsByPaymentIdOrMerchantId(PaymentId, null))
                                             .ReturnsAsync(paymentLog);

            // Act
            var response = await paymentService.GetDetailsAsync(null, PaymentId);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(paymentLog.MaskedCardNo, response.MaskedCardNo);
            Assert.AreEqual(paymentLog.Amount, response.Amount);
            Assert.AreEqual(paymentLog.Reason, response.BankMessage);
        }
    }
}
