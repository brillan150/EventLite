using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventCatalogApi.Migrations
{
    public partial class TicketType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_ticket_types_hilo",
                incrementBy: 10);

            migrationBuilder.AddColumn<int>(
                name: "TotalTicketLimitAllTypes",
                table: "CatalogEvents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CatalogTicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    TicketLimitThisType = table.Column<int>(nullable: false),
                    SalesEndOn = table.Column<DateTime>(nullable: false),
                    CatalogEventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTicketTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogTicketTypes_CatalogEvents_CatalogEventId",
                        column: x => x.CatalogEventId,
                        principalTable: "CatalogEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTicketTypes_CatalogEventId",
                table: "CatalogTicketTypes",
                column: "CatalogEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogTicketTypes");

            migrationBuilder.DropSequence(
                name: "catalog_ticket_types_hilo");

            migrationBuilder.DropColumn(
                name: "TotalTicketLimitAllTypes",
                table: "CatalogEvents");
        }
    }
}
