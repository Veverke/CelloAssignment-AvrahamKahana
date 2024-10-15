namespace PaymentService.DbContexts.Models
{
    public class ParkingLot
    {
        public int ParkingLotId { get; set; }
        public List<TariffPerHour> TariffsPerInitialHours { get; set; }
        public double MaxTariff { get; set; }
        public double MinTariff { get; set; }

        //Format: starting hour (in 00:00 format), ending hour (in 00:00 format), days span, tariff. Value delimiter: , Item delimiter: -
        public string SpecialHoursTariffOverride { get; set; }

        public ParkingLot()
        {
            TariffsPerInitialHours = new List<TariffPerHour>();
        }
    }
}
