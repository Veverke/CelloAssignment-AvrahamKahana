using Common;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.DbContexts;
using PaymentService.DbContexts.Models;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly ParkingPaymentCalculator _parkingPaymentCalculator;
        private readonly HttpClient _httpClient;
        public PaymentController(ILogger<PaymentController> logger, ParkingPaymentCalculator parkingPaymentCalculator, HttpClient httpClient)
        {
            _logger = logger;
            _parkingPaymentCalculator = parkingPaymentCalculator;
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> CalculateParkingCost([FromBody] ParkingTransactionDto parkingTransaction)
        {

            using var dbContext = new PaymentContext();
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                var matchingParkingLot = dbContext.ParkingLots
                    .Include(p => p.TariffsPerInitialHours)
                    .FirstOrDefault(p => p.ParkingLotId == parkingTransaction.ParkingLotId);

                if (matchingParkingLot is null)
                {
                    _logger.LogError($"No matching parking lot found for parking lod id [{parkingTransaction.ParkingLotId}]...");
                    return BadRequest();
                }

                var parkingCost = _parkingPaymentCalculator.Calculate(matchingParkingLot, parkingTransaction);

                var newInvoice = new Invoice
                {
                    CustomerId = parkingTransaction.CustomerId,
                    Charged = parkingCost,
                    GateOpened = parkingTransaction.GateOpened,
                    GateClosed = parkingTransaction.GateClosed,
                    ParkingLotId = parkingTransaction.ParkingLotId,
                    Timestamp = DateTime.Now
                };
                dbContext.Invoices.Add(newInvoice);

                //Mark transaction as completed in DB
                var response = await _httpClient.PostAsJsonAsync($"http://parking-service/api/Parking/CompleteParkingTransaction", parkingTransaction);
                await response.HandleResponseError(1);
                if (await response.HandleResponseError(1))
                {
                    return null;
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(newInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(PaymentService)}.{nameof(PaymentController)}.{nameof(CalculateParkingCost)}: [{ex.Message}]");
                transaction.Rollback();

                //Mark transaction as completed in DB
                var response = await _httpClient.PostAsJsonAsync($"http://parking-service/api/Parking/ResetParkingTransaction", parkingTransaction);
                await response.HandleResponseError(1);
                return StatusCode(500, parkingTransaction);
            }
        }
    }
}
