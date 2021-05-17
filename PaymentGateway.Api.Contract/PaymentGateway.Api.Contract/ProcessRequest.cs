using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PaymentGateway.Api.Contract
{
    [DataContract]
    public class ProcessRequest
    {
        [DataMember(Name = "cardNo")]
        [CreditCard]
        public string CardNo { get; set; } = null!;
        [DataMember(Name = "expiry")]
        public string Expiry { get; set; } = null!;
        [DataMember(Name = "cvv")]
        public int CVV { get; set; }
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; } = null!;
        [DataMember(Name = "merchantId")]
        public string MerchantId { get; set; } = null!;
    }
}
