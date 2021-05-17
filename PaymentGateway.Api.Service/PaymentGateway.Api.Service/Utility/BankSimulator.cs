using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Service.Utility
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
