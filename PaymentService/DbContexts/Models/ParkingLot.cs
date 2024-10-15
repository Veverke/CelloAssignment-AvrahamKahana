namespace PaymentService.DbContexts.Models
{
    //TODO: I opted to model parking lots as records in a database table. This means adding new parking lots is transparent, this can be done in parallel with the app running, offline, at night, for example. No need to change and release the code.
    //"Flattening" logic into table rows may be tricky, it does not allow the same flexibility as writing code. But has the trade-off mentioned above.
    //In order to model the logic supported by the 4 parking lots, I used a string field that serializes part of the logic. See string format description below and _parseTariffOverride local function.
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
