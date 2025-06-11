using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualCost",
                table: "Step",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Step",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Step",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedDate",
                table: "Step",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Step_CreatorId",
                table: "Step",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Step_AspNetUsers_CreatorId",
                table: "Step",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Step_AspNetUsers_CreatorId",
                table: "Step");

            migrationBuilder.DropIndex(
                name: "IX_Step_CreatorId",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "ActualCost",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "EstimatedDate",
                table: "Step");
        }
    }
}
