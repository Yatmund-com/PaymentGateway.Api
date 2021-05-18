using PaymentGateway.Api.Contract;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Operation
{
    public interface IOperations
    {
        Task<ResponseBase> AddPaymentAsync(ProcessRequest processRequest, BankResponse bankResponse, string paymentId);
        Task<PaymentLog?> GetPaymentDetailsByPaymentIdOrMerchantId(string? paymentId, string? merchantId);
    }
}
