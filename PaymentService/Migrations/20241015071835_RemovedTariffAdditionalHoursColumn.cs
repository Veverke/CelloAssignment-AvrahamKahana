using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Migrations
{
    public partial class RemovedTariffAdditionalHoursColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TariffPerAdditionalHour",
                table: "ParkingLots");

            migrationBuilder.InsertData(
                table: "ParkingLots",
                columns: new[] { "ParkingLotId", "MaxTariff", "MinTariff", "SpecialHoursTariffOverride" },
                values: new object[,]
                {
                    { 0, 0, 0, "" },
                    { 1, 0, 0, "05:00,07:00,0,30" },
                    { 2, 0, 0, "14:00,07:00,1,30" },
                    { 3, 35, 0, "" },
                });

            // Insert Tariff Per Hour data for Parking Lot 0
            migrationBuilder.InsertData(
                table: "TariffsPerHour",
                columns: new[] { "Id", "Hour", "Tariff", "ParkingLotId" },
                values: new object[,]
                {
                    { 1, 1, 16, 0 },
                    { 2, 2, 8, 0 },
                    { 3, -1, 6, 0 },
                });

            // Insert Tariff Per Hour data for Parking Lot 1
            migrationBuilder.InsertData(
                table: "TariffsPerHour",
                columns: new[] { "Id", "Hour", "Tariff", "ParkingLotId" },
                values: new object[,]
                {
                    { 4, 1, 10, 1 },
                    { 5, 2, 8, 1 },
                    { 6, -1, 6, 1 },
                });

            // Insert Tariff Per Hour data for Parking Lot 2
            migrationBuilder.InsertData(
                table: "TariffsPerHour",
                columns: new[] { "Id", "Hour", "Tariff", "ParkingLotId" },
                values: new object[,]
                {
                    { 7, 1, 16, 2 },
                    { 8, 2, 9, 2 },
                    { 9, -1, 6, 2 },
                });

            // Insert Tariff Per Hour data for Parking Lot 3
            migrationBuilder.InsertData(
                table: "TariffsPerHour",
                columns: new[] { "Id", "Hour", "Tariff", "ParkingLotId" },
                values: new object[,]
                {
                    { 10, 1, 15, 3 },
                    { 11, 2, 14, 3 },
                    { 12, -1, 13, 3 },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TariffPerAdditionalHour",
                table: "ParkingLots",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            // Delete ParkingLots first since they rely on TariffsPerHour
            migrationBuilder.DeleteData(
                table: "ParkingLots",
                keyColumn: "ParkingLotId",
                keyValue: 0);
            migrationBuilder.DeleteData(
                table: "ParkingLots",
                keyColumn: "ParkingLotId",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "ParkingLots",
                keyColumn: "ParkingLotId",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "ParkingLots",
                keyColumn: "ParkingLotId",
                keyValue: 3);

            // Then delete TariffsPerHour
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 3);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 4);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 5);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 6);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 7);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 8);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 9);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 10);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 11);
            migrationBuilder.DeleteData(
                table: "TariffsPerHour",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
