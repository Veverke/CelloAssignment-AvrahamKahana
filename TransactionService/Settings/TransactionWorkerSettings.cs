namespace TransactionService.Settings
{
    //Server settings can be found throughout objects named ****Settings, across the projects, like the one below.
    //They are injected in the respective components using IOptions pattern.
    public class TransactionWorkerSettings
    {
        public const string SectionName = "TransactionWorker";
        public int ReadDelayInSecs { get; set; }
        public int BatchItemsLimit { get; set; }
        public int MaxRetries { get; set; }
        public int RetryDelayInSecs { get; set; }
    }
}
