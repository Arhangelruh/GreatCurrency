using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBankServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SCRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncomingDate = table.Column<DateTime>(type: "Timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CSCurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USDBuyRate = table.Column<double>(type: "double precision", nullable: true),
                    USDSaleRate = table.Column<double>(type: "double precision", nullable: true),
                    EURBuyRate = table.Column<double>(type: "double precision", nullable: true),
                    EURSaleRate = table.Column<double>(type: "double precision", nullable: true),
                    RUBBuyRate = table.Column<double>(type: "double precision", nullable: true),
                    RUBSaleRate = table.Column<double>(type: "double precision", nullable: true),
                    CurrencyServicesId = table.Column<int>(type: "integer", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CSCurrencies_CurrencyServices_CurrencyServicesId",
                        column: x => x.CurrencyServicesId,
                        principalTable: "CurrencyServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CSCurrencies_SCRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "SCRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSCurrencies_CurrencyServicesId",
                table: "CSCurrencies",
                column: "CurrencyServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_CSCurrencies_RequestId",
                table: "CSCurrencies",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSCurrencies");

            migrationBuilder.DropTable(
                name: "CurrencyServices");

            migrationBuilder.DropTable(
                name: "SCRequests");
        }
    }
}
