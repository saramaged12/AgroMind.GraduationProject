using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeRelationsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_AspNetUsers_AppUserId",
                table: "Addresss");

            migrationBuilder.DropIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Addresss",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_AspNetUsers_AppUserId",
                table: "Addresss",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_AspNetUsers_AppUserId",
                table: "Addresss");

            migrationBuilder.DropIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Addresss",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_AspNetUsers_AppUserId",
                table: "Addresss",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
