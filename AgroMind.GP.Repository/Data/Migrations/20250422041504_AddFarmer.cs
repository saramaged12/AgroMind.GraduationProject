using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmerId",
                table: "Land",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FarmerId",
                table: "Crop",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Farmer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farmer_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Land_FarmerId",
                table: "Land",
                column: "FarmerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Land_Farmer_FarmerId",
                table: "Land",
                column: "FarmerId",
                principalTable: "Farmer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crop_Farmer_FarmerId",
                table: "Crop");

            migrationBuilder.DropForeignKey(
                name: "FK_Land_Farmer_FarmerId",
                table: "Land");

            migrationBuilder.DropTable(
                name: "Farmer");

            migrationBuilder.DropIndex(
                name: "IX_Land_FarmerId",
                table: "Land");

            migrationBuilder.DropIndex(
                name: "IX_Crop_FarmerId",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                table: "Crop");
        }
    }
}
