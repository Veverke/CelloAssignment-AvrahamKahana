namespace PaymentService.DbContexts.Models
{
    public class TariffPerHour
    {
        public int Id { get; set; }
        //Hour equalling to -1 indicates the tariff for additional hours
        public int Hour { get; set; }
        public double Tariff { get; set; }
        public ParkingLot ParkingLot { get; set; }
    }
}
