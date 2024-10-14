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

        public override string ToString()
        {
            return $"{nameof(Id)}: [{Id}] {nameof(CreationTime)}: [{CreationTime}] {nameof(CustomerId)}: [{CustomerId}] {nameof(ParkingLotId)}: [{ParkingLotId}]  {nameof(GateOpened)}: [{GateOpened}]  {nameof(GateClosed)}: [{GateClosed}]";
        }
    }
}
