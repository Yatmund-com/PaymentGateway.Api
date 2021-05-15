using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Contract;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("details")]
        [ProducesResponseType(typeof(DetailsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DetailsResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<DetailsResponse>> GetAsync([FromQuery] string? merchantId, [FromQuery] string? paymentId)
        {
            return new DetailsResponse
            {
                Success = true,
                MaskedCardNo = "****4555"
            };
        }

        [HttpPost]
        [Route("process")]
        [ProducesResponseType(typeof(DetailsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DetailsResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ProcessResponse>> ProcessAsync([FromBody] ProcessRequest request)
        {

            return new ProcessResponse
            {
                Success = true,
                PaymentId = request.CardNo
            };
        }
    }
}