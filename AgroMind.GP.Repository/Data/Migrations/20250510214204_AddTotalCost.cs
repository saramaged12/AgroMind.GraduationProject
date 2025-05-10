using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Crop",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_CropStage_TotalCost",
                table: "CropStage",
                column: "TotalCost")
                .Annotation("SqlServer:Include", new[] { "CropId" });

            migrationBuilder.CreateIndex(
                name: "IX_Crop_TotalCost",
                table: "Crop",
                column: "TotalCost")
                .Annotation("SqlServer:Include", new[] { "LastStartDate", "Duration" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CropStage_TotalCost",
                table: "CropStage");

            migrationBuilder.DropIndex(
                name: "IX_Crop_TotalCost",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Crop");
        }
    }
}
