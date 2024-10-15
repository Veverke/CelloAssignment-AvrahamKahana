using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingService.Migrations
{
    public partial class AddTransactionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.Now;

            migrationBuilder.InsertData(
                table: "ParkingTransactions",
                columns: new[] { "Id", "CreationTime", "CustomerId", "ParkingLotId", "GateOpened", "GateClosed" },
                values: new object[,]
                {
                    { 1, now.AddDays(-1), 1, 0, now.AddHours(-5), now },
                    { 2, now.AddDays(-1), 0, 1, now.AddHours(-5), now },
                    { 3, now.AddDays(-1), 1, 1, now.AddHours(-3), now },
                    { 4, now.AddDays(-1), 1, 2, now.AddHours(-20), now },
                    { 5, now.AddDays(-1), 1, 3, now.AddHours(-0.5), now },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkingTransactions",
                keyColumn: "Id",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "ParkingTransactions",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "ParkingTransactions",
                keyColumn: "Id",
                keyValue: 3);
            migrationBuilder.DeleteData(
                table: "ParkingTransactions",
                keyColumn: "Id",
                keyValue: 4);
            migrationBuilder.DeleteData(
                table: "ParkingTransactions",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
