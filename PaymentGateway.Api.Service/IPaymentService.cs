using PaymentGateway.Api.Contract;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Service
{
    public interface IPaymentService
    {
        Task<DetailsResponse?> GetDetailsAsync(string? merchantId, string? paymentId);
        Task<ProcessResponse> ProcessPaymentAsync(ProcessRequest processRequest);
    }
}
