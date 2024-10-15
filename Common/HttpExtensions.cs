namespace Common
{
    public static class HttpExtensions
    {
        public static async Task<bool> HandleResponseError(this HttpResponseMessage response, int errorDelayInSecs = 1)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseError = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Http request error: [{(int)response.StatusCode}: {response.StatusCode}] {responseError} uri: [{response.RequestMessage.RequestUri}]");

                //TODO: move setting to appsettings
                await Task.Delay(TimeSpan.FromSeconds(errorDelayInSecs));
                return true;
            }

            return false;
        }
    }
}
