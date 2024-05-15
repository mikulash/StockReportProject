using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.SQLite.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Ticker = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CUSIP = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FundName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndexRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FundId = table.Column<long>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<long>(type: "INTEGER", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Shares = table.Column<long>(type: "INTEGER", nullable: false),
                    MarketValue = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndexRecords_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndexRecords_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CUSIP",
                table: "Companies",
                column: "CUSIP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funds_FundName",
                table: "Funds",
                column: "FundName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndexRecords_CompanyId",
                table: "IndexRecords",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IndexRecords_FundId",
                table: "IndexRecords",
                column: "FundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndexRecords");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Funds");
        }
    }
}
