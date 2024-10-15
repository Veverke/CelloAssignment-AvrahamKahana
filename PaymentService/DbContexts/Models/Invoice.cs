using Contracts;

namespace PaymentService.DbContexts.Models
{
    public class Invoice : ParkingTransactionDto
    {
        public int Id { get; set; }
        public double Charged { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
