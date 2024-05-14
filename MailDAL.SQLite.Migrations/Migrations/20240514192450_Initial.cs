using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailDAL.SQLite.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailSubscribers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSubscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberPreferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FundName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    OutputType = table.Column<int>(type: "INTEGER", nullable: false),
                    MailSubscriberId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriberPreferences_MailSubscribers_MailSubscriberId",
                        column: x => x.MailSubscriberId,
                        principalTable: "MailSubscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailSubscribers_Email",
                table: "MailSubscribers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberPreferences_MailSubscriberId",
                table: "SubscriberPreferences",
                column: "MailSubscriberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriberPreferences");

            migrationBuilder.DropTable(
                name: "MailSubscribers");
        }
    }
}
