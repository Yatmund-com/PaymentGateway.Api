using System.Runtime.Serialization;

namespace PaymentGateway.Api.Contract
{
    [DataContract]
    public class BankRequest
    {
        public BankRequest(string bank, string cardNo, decimal amount)
        {
            Bank = bank;
            CardNo = cardNo;
            Amount = amount;
        }

        [DataMember(Name = "bank")]
        public string Bank { get; set; } = null!;
        [DataMember(Name = "cardNo")]
        public string CardNo { get; set; } = null!;
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
    }
}
