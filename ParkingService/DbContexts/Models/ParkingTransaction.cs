using Common;
using Contracts;

namespace ParkingService.DbContexts.Models
{
    public class ParkingTransaction : ParkingTransactionDto
    {
        public long Id { get; set; }
        public ParkingTransactionStatus Status { get; set; } = ParkingTransactionStatus.New;
    }
}
