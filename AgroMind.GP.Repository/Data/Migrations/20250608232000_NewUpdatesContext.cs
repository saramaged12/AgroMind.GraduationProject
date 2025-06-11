using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdatesContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_AspNetUsers_AppUserId",
                table: "Addresss");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_AspNetUsers_CreatorId",
                table: "Step");

            

            migrationBuilder.DropIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Addresss",
                newName: "CreatorId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Step",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Land",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Land",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CropStage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "CropStage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Crop",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Crop",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Brands",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Brands",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "street",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Lname",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Fname",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Addresss",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorId",
                table: "Products",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Land_CreatorId",
                table: "Land",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CropStage_CreatorId",
                table: "CropStage",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Crop_CreatorId",
                table: "Crop",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CreatorId",
                table: "Brands",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_CreatorId",
                table: "Addresss",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_AspNetUsers_CreatorId",
                table: "Addresss",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresss_AspNetUsers_Id",
                table: "Addresss",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_AspNetUsers_CreatorId",
                table: "Brands",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatorId",
                table: "Categories",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Crop_AspNetUsers_CreatorId",
                table: "Crop",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CropStage_AspNetUsers_CreatorId",
                table: "CropStage",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Land_AspNetUsers_CreatorId",
                table: "Land",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreatorId",
                table: "Products",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_AspNetUsers_CreatorId",
                table: "Step",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_AspNetUsers_CreatorId",
                table: "Addresss");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresss_AspNetUsers_Id",
                table: "Addresss");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_AspNetUsers_CreatorId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatorId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Crop_AspNetUsers_CreatorId",
                table: "Crop");

            migrationBuilder.DropForeignKey(
                name: "FK_CropStage_AspNetUsers_CreatorId",
                table: "CropStage");

            migrationBuilder.DropForeignKey(
                name: "FK_Land_AspNetUsers_CreatorId",
                table: "Land");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreatorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_AspNetUsers_CreatorId",
                table: "Step");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreatorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Land_CreatorId",
                table: "Land");

            migrationBuilder.DropIndex(
                name: "IX_CropStage_CreatorId",
                table: "CropStage");

            migrationBuilder.DropIndex(
                name: "IX_Crop_CreatorId",
                table: "Crop");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Brands_CreatorId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Addresss_CreatorId",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CropStage");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "CropStage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Addresss");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Addresss",
                newName: "AppUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Step",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "street",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Lname",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fname",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

           
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

            migrationBuilder.AddForeignKey(
                name: "FK_Step_AspNetUsers_CreatorId",
                table: "Step",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
