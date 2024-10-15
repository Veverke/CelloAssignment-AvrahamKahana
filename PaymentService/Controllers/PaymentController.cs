using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.DbContexts;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly ParkingPaymentCalculator _parkingPaymentCalculator;
        public PaymentController(ILogger<PaymentController> logger, ParkingPaymentCalculator parkingPaymentCalculator)
        {
            _logger = logger;
            _parkingPaymentCalculator = parkingPaymentCalculator;
        }

        [HttpPost]
        public IActionResult CalculateParkingCost([FromBody] ParkingTransactionDto parkingTransaction)
        {
            using var dbContext = new PaymentContext();

            var matchingParkingLot = dbContext.ParkingLots
                .Include(p => p.TariffsPerInitialHours)
                .FirstOrDefault(p => p.ParkingLotId == parkingTransaction.ParkingLotId);

            if (matchingParkingLot is null)
            {
                _logger.LogError($"No matching parking lot found for parking lod id [{parkingTransaction.ParkingLotId}]...");
                return BadRequest();
            }

            var parkingCost = _parkingPaymentCalculator.Calculate(matchingParkingLot, parkingTransaction);

            return Ok(new
            {
                parkingTransaction.ParkingLotId,
                parkingCost
            });
        }
    }
}
