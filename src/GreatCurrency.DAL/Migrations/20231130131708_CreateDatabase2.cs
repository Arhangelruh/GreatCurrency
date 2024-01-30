using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GreatCurrency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityName = table.Column<string>(type: "text", nullable: false),
                    CityURL = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncomingDate = table.Column<DateTime>(type: "Timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentAddress = table.Column<string>(type: "text", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDepartment_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankDepartment_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BestCurrency",
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
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestCurrency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BestCurrency_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BestCurrency_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
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
                    BankDepartmentId = table.Column<int>(type: "integer", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currency_BankDepartment_BankDepartmentId",
                        column: x => x.BankDepartmentId,
                        principalTable: "BankDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Currency_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankDepartment_BankId",
                table: "BankDepartment",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankDepartment_CityId",
                table: "BankDepartment",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BestCurrency_BankId",
                table: "BestCurrency",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BestCurrency_RequestId",
                table: "BestCurrency",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_BankDepartmentId",
                table: "Currency",
                column: "BankDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_RequestId",
                table: "Currency",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestCurrency");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "BankDepartment");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
