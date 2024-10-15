namespace TransactionService.Settings
{
    public class TransactionWorkerSettings
    {
        public const string SectionName = "TransactionWorker";
        public int ReadDelayInSecs { get; set; }
    }
}
