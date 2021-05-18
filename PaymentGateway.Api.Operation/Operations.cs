using Newtonsoft.Json;
using PaymentGateway.Api.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Operation
{
    public class Operations : IOperations
    {
        private const string DatabasePath = "Database";
        public Operations()
        {

        }

        public async Task<ResponseBase> AddPaymentAsync(ProcessRequest processRequest, BankResponse bankResponse, string paymentId)
        {
            Directory.CreateDirectory(GetFullDatabsePath());
            var writer = File.CreateText(Path.Combine(GetFullDatabsePath(), $"{paymentId}-{processRequest.MerchantId}.json"));
            var paymentLog = new PaymentLog
            {
                Amount = processRequest.Amount,
                CurrencyCode = processRequest.CurrencyCode,
                PaymentId = paymentId,
                Expiry = processRequest.Expiry,
                MaskedCardNo = CreditCardUtility.MaskCardNo(processRequest.CardNo),
                MerchantId = processRequest.MerchantId,
                Success = bankResponse.Success,
                Reason = bankResponse.Reason,
                TransactionId = bankResponse.TransactionId
            };

            try
            {
                using (writer)
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(paymentLog));
                }

                return new ResponseBase
                {
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ResponseBase
                {
                    Success = false,
                    ErrorMessage = e.Message,
                    Message = "Something went wrong when creating database entry"

                };
            }
        }

        public async Task<PaymentLog?> GetPaymentDetailsByPaymentIdOrMerchantId(string? paymentId, string? merchantId)
        {
            var path = GetFullDatabsePath();
            var files = Directory.EnumerateFiles(path);
            var file = files.FirstOrDefault(x => x.Contains(paymentId ?? merchantId));
            if (file == null)
                return null;

            var logString = await File.ReadAllTextAsync(file);

            return JsonConvert.DeserializeObject<PaymentLog>(logString);
        }

        private string GetFullDatabsePath()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory);
            var databaseDirectory = parentDirectory.EnumerateDirectories().FirstOrDefault(x => x.Name == DatabasePath);
            if(databaseDirectory == null)
            {
                databaseDirectory = parentDirectory.Parent.Parent.Parent.EnumerateDirectories().FirstOrDefault(x => x.Name == DatabasePath);
            }

            return databaseDirectory.FullName;
        }
    }
}
