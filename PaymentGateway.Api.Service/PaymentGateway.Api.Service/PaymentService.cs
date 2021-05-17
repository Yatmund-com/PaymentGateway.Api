using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Integration;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankIntegration _bankIntegration;
        private const string SuccessBank = "Success";
        private const string FailedBank = "PaymentFailed";

        public PaymentService(IBankIntegration bankIntegration)
        {
            _bankIntegration = bankIntegration;
        }

        public async Task<DetailsResponse> GetDetailsAsync(string? merchantId, string? paymentId)
        {
            return new DetailsResponse
            {
                Success = true,
                MaskedCardNo = "****4555"
            };
        }

        public async Task<ProcessResponse> ProcessPaymentAsync(ProcessRequest processRequest)
        {
            var bankRequest = new BankRequest(GetBank(processRequest.CardNo), processRequest.CardNo);

            var bankResponse = await _bankIntegration.SendtoAcquirerAsync(bankRequest);

            if(bankResponse.Success)
            {
                return new ProcessResponse
                {
                    Success = true,
                    PaymentId = Guid.NewGuid().ToString()
                };
            }

            return new ProcessResponse
            {
                Success = false,
                ErrorMessage = bankResponse.Reason,
                Message = "Something went wrong with transaction, please contact card issuer.",
                PaymentId = Guid.NewGuid().ToString()
            };
        }

        private string GetBank(string cardNo)
        {
            if(cardNo.StartsWith("4"))
                return SuccessBank;

            return FailedBank;
        }
    }
}
