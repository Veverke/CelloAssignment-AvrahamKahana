using Contracts;

namespace ParkingService.DbContexts.Models
{
    public class ParkingTransaction : ParkingTransactionDto
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
