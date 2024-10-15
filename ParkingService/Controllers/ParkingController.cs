using Microsoft.AspNetCore.Mvc;
using ParkingService.DbContexts;
using ParkingService.DbContexts.Models;

namespace ParkingService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(ILogger<ParkingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task PostParkingTransaction([FromBody] ParkingTransaction parkingTransaction)
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
                _logger.LogError(ex, $"Error in {nameof(PostParkingTransaction)}: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<ParkingTransaction>> GetParkingTransactions(DateTime maxCreationDate)
        {
            using var dbContext = new ParkingContext();
            using var transaction = dbContext.Database.BeginTransaction();

            IEnumerable<ParkingTransaction> parkingTransactions = null;

            #region Db Transaction

            try
            {
                var metadataRecord = dbContext.Metadata.FirstOrDefault();
                if (metadataRecord is null)
                {
                    metadataRecord = new Metadata { LastRead = DateTime.MinValue };
                    dbContext.Metadata.Add(metadataRecord);
                }

                parkingTransactions = dbContext.ParkingTransactions.Where(p => p.CreationTime < maxCreationDate && p.CreationTime >= metadataRecord.LastRead).ToList();
                var now = DateTime.Now;
                _logger.LogInformation($"[{parkingTransactions.Count()}] parking transactions fetched for max creation date as [{maxCreationDate.ToString("dd/MM/yyyy HH:mm:ss")}] Metadata last read changed from [{metadataRecord.LastRead}] to [{now}]...");
                metadataRecord.LastRead = now;

                dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, $"Error in {nameof(ParkingService)}.{nameof(ParkingController)}.{nameof(GetParkingTransactions)}: [{ex.Message}]");
            }

            #endregion Db Transaction

            return parkingTransactions;
        }
    }
}
