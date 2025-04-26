using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesForCrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaPlanted",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "CropHealthStatus",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "CropType",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "LandPlantedType",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "plantingDate",
                table: "Crop");

            migrationBuilder.AlterColumn<string>(
                name: "CropName",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CropName",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AreaPlanted",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CropHealthStatus",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CropType",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandPlantedType",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Crop",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "plantingDate",
                table: "Crop",
                type: "datetime2",
                nullable: true);
        }
    }
}
