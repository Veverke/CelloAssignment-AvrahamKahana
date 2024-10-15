using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingService.Migrations
{
    public partial class MetadataPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Metadata",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metadata",
                table: "Metadata",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Metadata",
                table: "Metadata");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Metadata");
        }
    }
}
