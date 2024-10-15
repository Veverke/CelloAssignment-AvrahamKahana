using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.Migrations
{
    public partial class PaymentServiceTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingLots",
                columns: table => new
                {
                    ParkingLotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TariffPerAdditionalHour = table.Column<double>(type: "float", nullable: false),
                    MaxTariff = table.Column<double>(type: "float", nullable: false),
                    MinTariff = table.Column<double>(type: "float", nullable: false),
                    SpecialHoursTariffOverride = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingLots", x => x.ParkingLotId);
                });

            migrationBuilder.CreateTable(
                name: "TariffPerHour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    Tariff = table.Column<double>(type: "float", nullable: false),
                    ParkingLotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffPerHour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffPerHour_ParkingLots_ParkingLotId",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLots",
                        principalColumn: "ParkingLotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffPerHour_ParkingLotId",
                table: "TariffPerHour",
                column: "ParkingLotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffPerHour");

            migrationBuilder.DropTable(
                name: "ParkingLots");
        }
    }
}
