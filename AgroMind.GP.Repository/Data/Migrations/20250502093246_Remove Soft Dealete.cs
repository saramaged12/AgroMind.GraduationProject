using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroMind.GP.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSoftDealete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
	            name: "DeletedAt",
	            table: "Crop");

			migrationBuilder.DropColumn(
				name: "IsDeleted",
				table: "Crop");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<DateTime>(
	           name: "DeletedAt",
	           table: "Crop",
	           nullable: true);

			migrationBuilder.AddColumn<bool>(
				name: "IsDeleted",
				table: "Crop",
				nullable: false,
				defaultValue: false);
		}
    }
}
