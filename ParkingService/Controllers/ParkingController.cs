using Microsoft.AspNetCore.Mvc;
using ParkingService.DbContexts;
using ParkingService.DbContexts.Models;

namespace ParkingService.Controllers
{
    [Route("api/[controller][action]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(ILogger<ParkingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task PostParkingTransaction(ParkingTransaction parkingTransaction)
        {
            try
            {
                using var dbContext = new ParkingContext();
                dbContext.ParkingTransactions.Add(parkingTransaction);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation($"Added parking transaction: [{parkingTransaction}]");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PostParkingTransaction)}: {ex.Message}");
            }
        }
    }
}
