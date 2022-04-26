using Microsoft.EntityFrameworkCore.Migrations;

namespace CityInfo.Infrastructure.Migrations
{
    public partial class Zavijava : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Eiffle Tower");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "TEiffle Tower");
        }
    }
}
