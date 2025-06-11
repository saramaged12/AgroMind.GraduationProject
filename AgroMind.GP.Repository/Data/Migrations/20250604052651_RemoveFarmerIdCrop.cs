using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFarmerIdCrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crop_Farmer_FarmerId",
                table: "Crop");

            migrationBuilder.DropIndex(
                name: "IX_Crop_FarmerId",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                table: "Crop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmerId",
                table: "Crop",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crop_FarmerId",
                table: "Crop",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crop_Farmer_FarmerId",
                table: "Crop",
                column: "FarmerId",
                principalTable: "Farmer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
