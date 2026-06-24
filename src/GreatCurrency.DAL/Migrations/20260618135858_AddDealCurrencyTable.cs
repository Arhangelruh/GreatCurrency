using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDealCurrencyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DealStockRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USDRate = table.Column<decimal>(type: "numeric", nullable: false),
                    EURRate = table.Column<decimal>(type: "numeric", nullable: false),
                    RUBRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CNYRate = table.Column<decimal>(type: "numeric", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealStockRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealStockRates_LERequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "LERequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealStockRates_RequestId",
                table: "DealStockRates",
                column: "RequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealStockRates");
        }
    }
}
