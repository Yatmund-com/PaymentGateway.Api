using PaymentGateway.Api.Contract;

namespace PaymentGateway.Api.Configuration
{
    public interface IConfigurationReader
    {
        BankConfiguration Read(string bank);
    }
}
