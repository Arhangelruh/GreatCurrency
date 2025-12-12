using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddConversionCurrencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EURRUBBuyRate",
                table: "Currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURRUBSellRate",
                table: "Currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURUSDBuyRate",
                table: "Currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURUSDSellRate",
                table: "Currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "USDRUBBuyRate",
                table: "Currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "USDRUBSellRate",
                table: "Currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURRUBBuyRate",
                table: "CSCurrencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURRUBSellRate",
                table: "CSCurrencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURUSDBuyRate",
                table: "CSCurrencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURUSDSellRate",
                table: "CSCurrencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "USDRUBBuyRate",
                table: "CSCurrencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "USDRUBSellRate",
                table: "CSCurrencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURRUBBuyRate",
                table: "BestCurrency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURRUBSellRate",
                table: "BestCurrency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURUSDBuyRate",
                table: "BestCurrency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EURUSDSellRate",
                table: "BestCurrency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "USDRUBBuyRate",
                table: "BestCurrency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "USDRUBSellRate",
                table: "BestCurrency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EURRUBBuyRate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "EURRUBSellRate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "EURUSDBuyRate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "EURUSDSellRate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "USDRUBBuyRate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "USDRUBSellRate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "EURRUBBuyRate",
                table: "CSCurrencies");

            migrationBuilder.DropColumn(
                name: "EURRUBSellRate",
                table: "CSCurrencies");

            migrationBuilder.DropColumn(
                name: "EURUSDBuyRate",
                table: "CSCurrencies");

            migrationBuilder.DropColumn(
                name: "EURUSDSellRate",
                table: "CSCurrencies");

            migrationBuilder.DropColumn(
                name: "USDRUBBuyRate",
                table: "CSCurrencies");

            migrationBuilder.DropColumn(
                name: "USDRUBSellRate",
                table: "CSCurrencies");

            migrationBuilder.DropColumn(
                name: "EURRUBBuyRate",
                table: "BestCurrency");

            migrationBuilder.DropColumn(
                name: "EURRUBSellRate",
                table: "BestCurrency");

            migrationBuilder.DropColumn(
                name: "EURUSDBuyRate",
                table: "BestCurrency");

            migrationBuilder.DropColumn(
                name: "EURUSDSellRate",
                table: "BestCurrency");

            migrationBuilder.DropColumn(
                name: "USDRUBBuyRate",
                table: "BestCurrency");

            migrationBuilder.DropColumn(
                name: "USDRUBSellRate",
                table: "BestCurrency");
        }
    }
}
