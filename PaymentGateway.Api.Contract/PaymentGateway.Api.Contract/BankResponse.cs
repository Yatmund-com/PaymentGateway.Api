using System.Runtime.Serialization;

namespace PaymentGateway.Api.Contract
{
    [DataContract]
    public class BankResponse
    {
        [DataMember(Name = "transactionId")]
        public string TransactionId { get; set; } = null!;
        [DataMember(Name = "sucess")]
        public bool Success { get; set; }
        [DataMember(Name = "reason")]
        public string Reason { get; set; } = null!;

    }
}
