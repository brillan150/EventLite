using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventCatalogApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_event_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_format_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_topic_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "CatalogFormats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Format = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogTopics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Topic = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Start = table.Column<DateTime>(maxLength: 100, nullable: false),
                    End = table.Column<DateTime>(maxLength: 100, nullable: false),
                    PictureUrl = table.Column<string>(nullable: true),
                    VenueName = table.Column<string>(maxLength: 100, nullable: false),
                    VenueAddressLine1 = table.Column<string>(maxLength: 100, nullable: false),
                    VenueAddressLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    VenueAddressLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    VenueCity = table.Column<string>(maxLength: 100, nullable: false),
                    VenueStateProvince = table.Column<string>(fixedLength: true, maxLength: 2, nullable: false),
                    VenuePostalCode = table.Column<string>(fixedLength: true, maxLength: 5, nullable: false),
                    VenueMapUrl = table.Column<string>(nullable: true),
                    HostOrganizer = table.Column<string>(maxLength: 100, nullable: false),
                    CatalogFormatId = table.Column<int>(nullable: false),
                    CatalogTopicId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogEvents_CatalogFormats_CatalogFormatId",
                        column: x => x.CatalogFormatId,
                        principalTable: "CatalogFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogEvents_CatalogTopics_CatalogTopicId",
                        column: x => x.CatalogTopicId,
                        principalTable: "CatalogTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogEvents_CatalogFormatId",
                table: "CatalogEvents",
                column: "CatalogFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogEvents_CatalogTopicId",
                table: "CatalogEvents",
                column: "CatalogTopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogEvents");

            migrationBuilder.DropTable(
                name: "CatalogFormats");

            migrationBuilder.DropTable(
                name: "CatalogTopics");

            migrationBuilder.DropSequence(
                name: "catalog_event_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_format_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_topic_hilo");
        }
    }
}
