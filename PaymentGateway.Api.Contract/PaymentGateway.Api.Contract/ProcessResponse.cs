using System.Runtime.Serialization;

namespace PaymentGateway.Api.Contract
{
    [DataContract]
    public class ProcessResponse : ResponseBase
    {
        [DataMember(Name = "paymentId")]
        public string PaymentId { get; set; } = null!;
    }
}
