namespace PaymentService.DbContexts.Models
{
    public class TariffPerHour
    {
        public int Id { get; set; }
        public int Hour { get; set; }
        public double Tariff { get; set; }
        public ParkingLot ParkingLot { get; set; }
    }
}
