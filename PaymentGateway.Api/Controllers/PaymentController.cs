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
        public async Task<ActionResult<DetailsResponse>> GetAsync([FromQuery] string? merchantId, [FromQuery] string? paymentId)
        {
            return await _paymentService.GetDetailsAsync(merchantId, paymentId);
        }

        [HttpPost]
        [Route("process")]
        [ProducesResponseType(typeof(ProcessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProcessResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ProcessResponse>> ProcessAsync([FromBody] ProcessRequest request)
        {

            return await _paymentService.ProcessPaymentAsync(request);
        }
    }
}