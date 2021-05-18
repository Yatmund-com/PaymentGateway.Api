using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Contract;
using PaymentGateway.Api.Service;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("payment")]

    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("details")]
        [ProducesResponseType(typeof(DetailsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DetailsResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetailsAsync([FromQuery] string? merchantId, [FromQuery] string? paymentId)
        {
            using (_logger.BeginScope("Getting payment details for Payment Id {PaymentId} and/or Merchant Id {MerchantId}", paymentId, merchantId))
            {
                var response = await _paymentService.GetDetailsAsync(merchantId, paymentId);
                if (response != null)
                {
                    return Ok(response);
                }

                return NotFound(new DetailsResponse { Message = "Payment not found, please check merchant or payment id again.", ErrorMessage = "Payment not found" });
            }
        }

        [HttpPost]
        [Route("process")]
        [ProducesResponseType(typeof(ProcessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProcessResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ProcessAsync([FromBody] ProcessRequest request)
        {
            using (_logger.BeginScope("Processing payment for Merchant Id {MerchantId} and Amount {Amount}", request.MerchantId, request.Amount))
            {
                var response = await _paymentService.ProcessPaymentAsync(request);
                if (response.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
        }
    }
}