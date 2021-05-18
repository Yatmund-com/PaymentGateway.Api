using System.Runtime.Serialization;

namespace PaymentGateway.Api.Contract
{
    [DataContract]
    public class ResponseBase
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; } = null!;
        [DataMember(Name = "message")]
        public string Message { get; set; } = null!;

    }
}
