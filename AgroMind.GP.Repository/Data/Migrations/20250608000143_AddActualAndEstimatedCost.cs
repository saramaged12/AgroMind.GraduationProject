using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActualAndEstimatedCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Step",
                newName: "EstimatedCost");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "CropStage",
                newName: "TotalEstimatedCost");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "CropStage",
                newName: "TotalActualCost");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "Crop",
                newName: "TotalEstimatedCost");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualCost",
                table: "CropStage",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedCost",
                table: "CropStage",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PlanType",
                table: "Crop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalActualCost",
                table: "Crop",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCost",
                table: "CropStage");

            migrationBuilder.DropColumn(
                name: "EstimatedCost",
                table: "CropStage");

            migrationBuilder.DropColumn(
                name: "PlanType",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "TotalActualCost",
                table: "Crop");

            migrationBuilder.RenameColumn(
                name: "EstimatedCost",
                table: "Step",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "TotalEstimatedCost",
                table: "CropStage",
                newName: "TotalCost");

            migrationBuilder.RenameColumn(
                name: "TotalActualCost",
                table: "CropStage",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "TotalEstimatedCost",
                table: "Crop",
                newName: "TotalCost");
        }
    }
}
