namespace PaymentService.DbContexts.Models
{
    public class ParkingLot
    {
        public int ParkingLotId { get; set; }
        public List<TariffPerHour> TariffsPerInitialHours { get; set; }
        public double TariffPerAdditionalHour { get; set; }
        public double MaxTariff { get; set; }
        public double MinTariff { get; set; }

        //Format: starting hour, ending hour, tariff. Value delimiter: , Item delimiter: -
        public string SpecialHoursTariffOverride { get; set; }

        public ParkingLot()
        {
            TariffsPerInitialHours = new List<TariffPerHour>();
        }
    }
}
