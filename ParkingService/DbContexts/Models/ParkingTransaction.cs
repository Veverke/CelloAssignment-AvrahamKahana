using Common;
using Contracts;

namespace ParkingService.DbContexts.Models
{
    public class ParkingTransaction : ParkingTransactionDto
    {
        public long Id { get; set; }
        //TODO: I added the status field to help me manage concurrency between different instances of the service racing to fetch transactions to process.
        //Once the a transaction is completed, its status moves to Completed.
        //New transactions are created with status New.
        //Once a batch of transactions is fetched, its status changes to Processing.
        //When fetching transactions, only those with status New and which LastExecutionAttempt is greater or equal to 30 mins from now - will be fetched.
        //I am assuming a database read automatically locks the data read and I perform the read + status upadate to Processing as a single transaction.
        public ParkingTransactionStatus Status { get; set; } = ParkingTransactionStatus.New;
        public DateTime LastExecutionAttempt { get; set; }
    }
}
