using Contracts;
using Microsoft.Extensions.Options;
using PaymentService.DbContexts.Models;
using PaymentService.Settings;

namespace PaymentService
{
    public class ParkingPaymentCalculator
    {
        private readonly ILogger<ParkingPaymentCalculator> _logger;
        private readonly TariffOverrideParseSettings _tariffOverrideParseSettings;
        public ParkingPaymentCalculator(ILogger<ParkingPaymentCalculator> logger, IOptions<TariffOverrideParseSettings> tariffOverrideParseSettings)
        {
            _logger = logger;
            _tariffOverrideParseSettings = tariffOverrideParseSettings?.Value;
        }

        public double Calculate(ParkingLot parkingLot, ParkingTransactionDto parkingTransaction)
        {
            List<(int startHour, int endHour, int daysSpan, double tariff)> _parseTariffOverride(string tariffOverrideStr)
            {
                var overrideTariffs = new List<(int startHour, int endHour, int daysSpan, double tariff)>();
                var overrideItems = tariffOverrideStr.Split(_tariffOverrideParseSettings.ItemDelimiter);
                foreach (var overrideItem in overrideItems)
                {
                    var overrideItemFields = overrideItem.Split(_tariffOverrideParseSettings.FieldDelimiter);
                    int.TryParse(overrideItemFields?.ElementAt(0), out var startHour);
                    int.TryParse(overrideItemFields?.ElementAt(1), out var endHour);
                    int.TryParse(overrideItemFields?.ElementAt(2), out var daysSpan);
                    double.TryParse(overrideItemFields?.ElementAt(3), out var tariff);
                    overrideTariffs.Add((startHour, endHour, daysSpan, tariff));
                }

                return overrideTariffs;
            }

            var parkingHoursTimeSpan = parkingTransaction.GateClosed - parkingTransaction.GateOpened;
            double parkingCost = 0;

            //cost per hour
            for (var hour = 1; hour <= parkingHoursTimeSpan.Hours; hour++)
            {
                var matchingHourTariff = parkingLot.TariffsPerInitialHours.FirstOrDefault(t => t.Hour == hour);
                var additionalHourTariff = parkingLot.TariffsPerInitialHours.FirstOrDefault(t => t.Hour == -1)?.Tariff ?? 0;

                //if hour falls into additional hours tariff
                if (matchingHourTariff is null)
                {
                    if (additionalHourTariff > 0)
                    {
                        _logger.LogInformation($"Incurred additional hour cost of: [{additionalHourTariff}] for hour [{hour}]");
                        parkingCost += additionalHourTariff;
                    }

                    continue;
                }

                _logger.LogInformation($"Incurred specific hour cost of: [{matchingHourTariff.Tariff}] for hour [{hour}]");
                parkingCost += matchingHourTariff.Tariff;
            }

            //ensure cost does not exceed maximum tariff, if applicable
            if (parkingLot.MaxTariff > 0)
            {
                _logger.LogInformation($"Cost changed from [{parkingCost}] to max limit of: [{parkingLot.MaxTariff}]");
                parkingCost = Math.Min(parkingCost, parkingLot.MaxTariff);
            }

            //apply cost override, if present
            var overrideTariffs = _parseTariffOverride(parkingLot.SpecialHoursTariffOverride);
            foreach (var overrideTariff in overrideTariffs)
            {
                if (parkingTransaction.GateOpened.Hour >= overrideTariff.startHour &&
                    parkingTransaction.GateOpened.Hour <= overrideTariff.endHour)
                {
                    _logger.LogInformation($"Cost override rule existent for start hour: [{overrideTariff.startHour}] end hour: [{overrideTariff.endHour}]. Changed from [{parkingCost}] to [{overrideTariff.tariff}]");
                    parkingCost = overrideTariff.tariff;
                    break;
                }
            }

            _logger.LogInformation($"Final parking cost for parking lot id: [{parkingTransaction.ParkingLotId}] gate opened: [{parkingTransaction.GateOpened}] gate closed: [{parkingTransaction.GateClosed}]: {parkingCost}");
            return parkingCost;
        }
    }
}
