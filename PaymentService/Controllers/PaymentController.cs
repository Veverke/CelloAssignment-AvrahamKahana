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

        [HttpGet]
        public IActionResult CalculateParkingCost(int parkingLotId)
        {
            return Ok($"Parking lot [{parkingLotId}] cost is: [{0}]");
        }
    }
}
