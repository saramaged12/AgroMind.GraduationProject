using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesLand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Land");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Land",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Land");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Land",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Land",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
