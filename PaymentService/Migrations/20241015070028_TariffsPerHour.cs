using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Migrations
{
    public partial class TariffsPerHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //TODO: the 4 parking lot models + each of its special tariff logic come already added to the database via some of my EF migrations.
            //In order to browse the database, you can use Visual Studio SQL Server Explorer, and connect to sql-server,1433, user sa, password StrongPassword123!
            migrationBuilder.DropForeignKey(
                name: "FK_TariffPerHour_ParkingLots_ParkingLotId",
                table: "TariffPerHour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TariffPerHour",
                table: "TariffPerHour");

            migrationBuilder.RenameTable(
                name: "TariffPerHour",
                newName: "TariffsPerHour");

            migrationBuilder.RenameIndex(
                name: "IX_TariffPerHour_ParkingLotId",
                table: "TariffsPerHour",
                newName: "IX_TariffsPerHour_ParkingLotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TariffsPerHour",
                table: "TariffsPerHour",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffsPerHour_ParkingLots_ParkingLotId",
                table: "TariffsPerHour",
                column: "ParkingLotId",
                principalTable: "ParkingLots",
                principalColumn: "ParkingLotId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TariffsPerHour_ParkingLots_ParkingLotId",
                table: "TariffsPerHour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TariffsPerHour",
                table: "TariffsPerHour");

            migrationBuilder.RenameTable(
                name: "TariffsPerHour",
                newName: "TariffPerHour");

            migrationBuilder.RenameIndex(
                name: "IX_TariffsPerHour_ParkingLotId",
                table: "TariffPerHour",
                newName: "IX_TariffPerHour_ParkingLotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TariffPerHour",
                table: "TariffPerHour",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffPerHour_ParkingLots_ParkingLotId",
                table: "TariffPerHour",
                column: "ParkingLotId",
                principalTable: "ParkingLots",
                principalColumn: "ParkingLotId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
