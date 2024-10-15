using Common;
using Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TransactionService.Settings;

namespace TransactionService
{
    //TODO: this is the "internal timer for processing transactions"
    public class TransactionWorker : BackgroundService
    {
        private readonly ILogger<TransactionWorker> _logger;
        private readonly HttpClient _httpClient;
        private readonly TransactionWorkerSettings _transactionWorkerSettings;
        public TransactionWorker(ILogger<TransactionWorker> logger, HttpClient httpClient, IOptions<TransactionWorkerSettings> transactionWorkerSettings)
        {
            _logger = logger;
            _httpClient = httpClient;
            _transactionWorkerSettings = transactionWorkerSettings?.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var parkingTransactions = await GetParkingTransactions(_transactionWorkerSettings.BatchItemsLimit);
                    //TODO: Parallel foreach
                    foreach (var parkingTransaction in parkingTransactions)
                    {
                        try
                        {
                            var invoiceDto = await CalculateParkingCost(parkingTransaction);
                            Console.WriteLine($"Parking cost for parking lot id [{parkingTransaction.ParkingLotId}]: [{invoiceDto?.Charged}]");

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error in processing transaction [{parkingTransaction}]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error in {nameof(TransactionService)}.{nameof(TransactionWorker)}.{nameof(ExecuteAsync)}: [{ex.Message}]");
                }


                await Task.Delay(TimeSpan.FromSeconds(_transactionWorkerSettings.ReadDelayInSecs));
            }
        }

        #region Private Methods
        private async Task<IEnumerable<ParkingTransactionDto>> GetParkingTransactions(int batchItemsLimit)
        {
            //TODO: example of exponential retry strategy. I could/should be using Polly instead.
            for (var i = 0; i < _transactionWorkerSettings.MaxRetries; i++)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"http://parking-service/api/Parking/GetParkingTransactions?batchItemsLimit={batchItemsLimit}");
                    if (await response.HandleResponseError(1))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(1, 2)));
                        continue;
                    }

                    var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<ParkingTransactionDto>>();
                    return responseContent;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed getting parking trasanctions: attempt {i + 1} out of {_transactionWorkerSettings.MaxRetries}...");
                }

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(1, 2)));
            }

            return Enumerable.Empty<ParkingTransactionDto>();
        }

        private async Task<InvoiceDto> CalculateParkingCost(ParkingTransactionDto parkingTransaction)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"http://payment-service/api/Payment/CalculateParkingCost", parkingTransaction);
                await response.HandleResponseError(1);
                if (await response.HandleResponseError(1))
                {
                    return null;
                }

                var invoiceDto = await response.Content.ReadFromJsonAsync<InvoiceDto>();
                return invoiceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(TransactionService)}.{nameof(TransactionWorker)}.{nameof(ExecuteAsync)}: [{ex.Message}]");
                return null;
            }
        }
        #endregion Private Methods
    }
}
