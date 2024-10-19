using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyService : Migration
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CSCurrencies_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
        }
    }
}
