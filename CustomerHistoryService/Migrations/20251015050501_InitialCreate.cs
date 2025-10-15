using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerHistoryService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationRecord_Histories_CustomerHistoryId",
                        column: x => x.CustomerHistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRecord_Histories_CustomerHistoryId",
                        column: x => x.CustomerHistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReservationRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationRecord_Histories_CustomerHistoryId",
                        column: x => x.CustomerHistoryId,
                        principalTable: "Histories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecord_CustomerHistoryId",
                table: "NotificationRecord",
                column: "CustomerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecord_CustomerHistoryId",
                table: "PaymentRecord",
                column: "CustomerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRecord_CustomerHistoryId",
                table: "ReservationRecord",
                column: "CustomerHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationRecord");

            migrationBuilder.DropTable(
                name: "PaymentRecord");

            migrationBuilder.DropTable(
                name: "ReservationRecord");

            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
