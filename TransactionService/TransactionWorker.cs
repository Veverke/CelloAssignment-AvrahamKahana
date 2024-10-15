using Common;
using Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using TransactionService.Settings;

namespace TransactionService
{
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
                    var parkingTransactions = await GetParkingTransactions();
                    //TODO: Parallel foreach
                    foreach (var parkingTransaction in parkingTransactions)
                    {
                        var parkingCost = await CalculateParkingCost(parkingTransaction);
                        Console.WriteLine($"Parking cost for parking lot id [{parkingTransaction.ParkingLotId}]: [{parkingCost}]");
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
        private async Task<IEnumerable<ParkingTransactionDto>> GetParkingTransactions()
        {
            var response = await _httpClient.GetAsync($"http://parking-service/api/Parking/GetParkingTransactions?maxCreationDate={DateTime.Now/*.ToString("dd-MM-yyyy HH:mm:ss")*/}");
            if (await response.HandleResponseError(1))
            {
                return Enumerable.Empty<ParkingTransactionDto>();
            }

            var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<ParkingTransactionDto>>();
            return responseContent;
        }

        private async Task<double> CalculateParkingCost(ParkingTransactionDto parkingTransaction)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"http://payment-service/api/Payment/CalculateParkingCost", JsonConvert.SerializeObject(parkingTransaction));
                await response.HandleResponseError(1);
                if (await response.HandleResponseError(1))
                {
                    return 0;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!double.TryParse(responseContent, out var parsedContentAsDouble))
                {
                    _logger.LogError($"Error in parsing {nameof(CalculateParkingCost)} result: [{responseContent}] into double...");
                    return 0;
                }
                Console.WriteLine($"Response from Payment Service: [{responseContent}]");
                return parsedContentAsDouble;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(TransactionService)}.{nameof(TransactionWorker)}.{nameof(ExecuteAsync)}: [{ex.Message}]");
                return 0;
            }
        }
        #endregion Private Methods
    }
}
