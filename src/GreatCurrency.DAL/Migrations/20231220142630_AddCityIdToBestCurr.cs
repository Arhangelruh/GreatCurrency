using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCityIdToBestCurr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "BestCurrency",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BestCurrency_CityId",
                table: "BestCurrency",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BestCurrency_City_CityId",
                table: "BestCurrency",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BestCurrency_City_CityId",
                table: "BestCurrency");

            migrationBuilder.DropIndex(
                name: "IX_BestCurrency_CityId",
                table: "BestCurrency");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "BestCurrency");
        }
    }
}
