using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLegalEntitiesRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LERequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncomingDate = table.Column<DateTime>(type: "Timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LERequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LECurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USDBuyRate = table.Column<double>(type: "double precision", nullable: false),
                    USDSaleRate = table.Column<double>(type: "double precision", nullable: false),
                    EURBuyRate = table.Column<double>(type: "double precision", nullable: false),
                    EURSaleRate = table.Column<double>(type: "double precision", nullable: false),
                    RUBBuyRate = table.Column<double>(type: "double precision", nullable: false),
                    RUBSaleRate = table.Column<double>(type: "double precision", nullable: false),
                    CNYBuyRate = table.Column<double>(type: "double precision", nullable: false),
                    CNYSaleRate = table.Column<double>(type: "double precision", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LECurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LECurrencies_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LECurrencies_LERequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "LERequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LECurrencies_BankId",
                table: "LECurrencies",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_LECurrencies_RequestId",
                table: "LECurrencies",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LECurrencies");

            migrationBuilder.DropTable(
                name: "LERequests");
        }
    }
}
