using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TransactionService
{
    public class TransactionWorker : BackgroundService
    {
        private readonly ILogger<TransactionWorker> _logger;
        private readonly HttpClient _httpClient;
        public TransactionWorker(ILogger<TransactionWorker> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            long iteration = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"Running Worker... iteration: [{++iteration}]");
                    var response = await _httpClient.GetAsync("http://payment-service/api/Payment/CalculateParkingCost?parkingLotId=5");
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseError = await response.Content.ReadAsStringAsync();
                        var errorMessage = $"Http request error: [{(int)response.StatusCode}: {response.StatusCode}] {responseError} uri: [{response.RequestMessage.RequestUri}]";
                        _logger.LogError(errorMessage);

                        //TODO: move setting to appsettings
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        continue;

                    }
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response from Payment Service: [{responseContent}]");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in {nameof(TransactionService)}.{nameof(TransactionWorker)}.{nameof(ExecuteAsync)}: [{ex.Message}]");
                }

                //TODO: move setting to appsettings
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
