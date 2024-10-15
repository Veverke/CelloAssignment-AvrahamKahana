using Common;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ParkingService.DbContexts;
using ParkingService.DbContexts.Models;
using ParkingService.Settings;

namespace ParkingService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;
        private readonly ParkingControllerSettings _parkingControllerSettings;

        public ParkingController(ILogger<ParkingController> logger, IOptions<ParkingControllerSettings> parkingControllerSettings)
        {
            _logger = logger;
            _parkingControllerSettings = parkingControllerSettings?.Value;
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
        public async Task<IEnumerable<ParkingTransaction>> GetParkingTransactions(int batchItemsLimit)
        {
            using var dbContext = new ParkingContext();
            using var transaction = dbContext.Database.BeginTransaction();

            IEnumerable<ParkingTransaction> parkingTransactions = null;

            #region Db Transaction

            try
            {
                //TODO: transactions that fail to process are reset to status => New and have their LastExecutionAttempt field set to the time the execution took place, ensuring that the Transaction Worker will not fetch transactions that were processed in the last 30 mins)
                parkingTransactions = dbContext.ParkingTransactions
                    .Where(p => p.Status == ParkingTransactionStatus.New &&
                                (DateTime.Now - p.LastExecutionAttempt) >= TimeSpan.FromMinutes(_parkingControllerSettings.TransactionNextAttemptRetryInMins)
                    )
                    .Take(batchItemsLimit)
                    .ToList();

                _logger.LogInformation($"[{parkingTransactions.Count()}] parking transactions fetched... batch items limit: [{batchItemsLimit}]");

                foreach (var parkingTransaction in parkingTransactions)
                {
                    parkingTransaction.Status = ParkingTransactionStatus.Processing;
                }

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

        [HttpPost]
        public async Task<bool> CompleteParkingTransaction([FromBody] ParkingTransactionDto parkingTrasaction)
        {
            return await _updateParkingTransactionStatus(parkingTrasaction, ParkingTransactionStatus.Completed);
        }

        [HttpPost]
        public async Task<bool> ResetParkingTransaction([FromBody] ParkingTransactionDto parkingTrasaction)
        {
            return await _updateParkingTransactionStatus(parkingTrasaction, ParkingTransactionStatus.New);
        }

        private async Task<bool> _updateParkingTransactionStatus(ParkingTransactionDto parkingTransaction, ParkingTransactionStatus status)
        {
            try
            {
                using var dbContext = new ParkingContext();
                var matchingTransaction = dbContext.ParkingTransactions
                    .FirstOrDefault(p =>
                        p.ParkingLotId == parkingTransaction.ParkingLotId &&
                        p.CustomerId == parkingTransaction.CustomerId &&
                        p.GateOpened == parkingTransaction.GateOpened &&
                        p.GateClosed == parkingTransaction.GateClosed);

                if (matchingTransaction is null)
                {
                    _logger.LogError($"No matching transaction found in {nameof(CompleteParkingTransaction)} for transaction: [{parkingTransaction}]");
                    return false;
                }

                matchingTransaction.Status = status;
                matchingTransaction.LastExecutionAttempt = DateTime.Now;
                _logger.LogInformation($"Updated processing parking transaction: [{matchingTransaction}]");
                return await dbContext.SaveChangesAsync() > 0;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(_updateParkingTransactionStatus)}: {ex.Message}");
                return false;

            }
        }
    }
}
