using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CalculateParkingCost([FromBody] ParkingTransactionDto parkingTransaction)
        {
            return Ok($"Parking lot [{parkingTransaction.ParkingLotId}] cost is: [{0}]");
        }
    }
}
