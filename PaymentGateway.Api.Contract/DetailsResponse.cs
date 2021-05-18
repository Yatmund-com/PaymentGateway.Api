using System.Runtime.Serialization;

namespace PaymentGateway.Api.Contract
{
    [DataContract]
    public class DetailsResponse : ResponseBase
    {
        [DataMember(Name = "maskedCardNo")]
        public string MaskedCardNo { get; set; } = null!;
        [DataMember(Name = "expiry")]
        public string Expiry { get; set; } = null!;
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; } = null!;
        [DataMember]
        public string BankMessage { get; set; } = null!;
    }
}
