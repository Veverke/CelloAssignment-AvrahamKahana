namespace ParkingService.DbContexts.Models
{
    public class ParkingTransaction
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public int CustomerId { get; set; }
        public int ParkingLotId { get; set; }
        public DateTime GateOpened { get; set; }
        public DateTime GateClosed { get; set; }
    }
}
