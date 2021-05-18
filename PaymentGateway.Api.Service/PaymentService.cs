using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Integration;
using PaymentGateway.Api.Operation;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankIntegration _bankIntegration;
        private readonly IOperations _operations;
        private readonly ILogger<PaymentService> _logger;
        private const string SuccessBank = "Success";
        private const string FailedBank = "PaymentFailed";

        public PaymentService(IBankIntegration bankIntegration, IOperations operations, ILogger<PaymentService> logger)
        {
            _bankIntegration = bankIntegration;
            _operations = operations;
            _logger = logger;
        }

        public async Task<DetailsResponse?> GetDetailsAsync(string? merchantId, string? paymentId)
        {
            var log = await _operations.GetPaymentDetailsByPaymentIdOrMerchantId(paymentId, merchantId);
            if (log != null)
            {
                return CreateDetailsResponse(log);
            }

            return null;
        }

        public async Task<ProcessResponse> ProcessPaymentAsync(ProcessRequest processRequest)
        {
            var paymentId = Guid.NewGuid().ToString();
            var bankRequest = new BankRequest(GetBank(processRequest.CardNo), processRequest.CardNo);

            var bankResponse = await _bankIntegration.SendtoAcquirerAsync(bankRequest);

            var dbResponse = await _operations.AddPaymentAsync(processRequest, bankResponse, paymentId);
            if (!dbResponse.Success)
            {
                _logger.LogError("Failed to log payment to databse for Payment Id {PaymentId}", paymentId);
            }

            if (bankResponse.Success)
            {
                return new ProcessResponse
                {
                    Success = true,
                    PaymentId = paymentId
                };
            }

            _logger.LogWarning("Acquirer returned error for Payment Id {PaymentId}", paymentId);
            return new ProcessResponse
            {

                Success = false,
                ErrorMessage = bankResponse.Reason,
                Message = "Something went wrong with transaction, please contact card issuer.",
                PaymentId = paymentId
            };
        }

        private string GetBank(string cardNo)
        {
            if (cardNo.StartsWith("5"))
                return FailedBank;

            return SuccessBank;
        }

        private DetailsResponse CreateDetailsResponse(PaymentLog paymentLog)
        {
            return new DetailsResponse
            {
                Success = paymentLog.Success,
                Amount = paymentLog.Amount,
                CurrencyCode = paymentLog.CurrencyCode,
                BankMessage = paymentLog.Reason,
                Expiry = paymentLog.Expiry,
                MaskedCardNo = paymentLog.MaskedCardNo
            };
        }
    }
}
