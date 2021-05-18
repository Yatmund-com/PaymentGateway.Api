namespace PaymentGateway.Api.Contract
{
    public class PaymentLog
    {
        public string MaskedCardNo { get; set; } = null!;
        public string Expiry { get; set; } = null!;
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string MerchantId { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
        public bool Success { get; set; }
        public string Reason { get; set; } = null!;
        public string PaymentId { get; set; } = null!;
    }
}
