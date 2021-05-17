using PaymentGateway.Api.Contract;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Integration
{
    public interface IBankIntegration
    {
        Task<BankResponse> SendtoAcquirerAsync(BankRequest bankRequest);
    }
}
