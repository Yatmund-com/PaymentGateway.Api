namespace PaymentGateway.Api.Operation
{
    public static class CreditCardUtility
    {
        public static string MaskCardNo(string cardNo)
        {
            var maskedCardNo = string.Empty;
            var cardIndexStart = cardNo.Length - 4;

            for (int i = 0; i < cardIndexStart; i++)
            {
                maskedCardNo += "*";
            }
            return maskedCardNo += cardNo.Substring(cardIndexStart);
        }
    }
}
