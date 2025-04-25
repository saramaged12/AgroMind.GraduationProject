using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLandRemoveWeatherConditionandStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "weatherCondition",
                table: "Land");

            migrationBuilder.RenameColumn(
                name: "AreaSize",
                table: "Land",
                newName: "Size");

            migrationBuilder.AlterColumn<string>(
                name: "IrrigationType",
                table: "Land",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "LandName",
                table: "Land",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandName",
                table: "Land");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Land",
                newName: "AreaSize");

            migrationBuilder.AlterColumn<string>(
                name: "IrrigationType",
                table: "Land",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Land",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Land",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "weatherCondition",
                table: "Land",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
