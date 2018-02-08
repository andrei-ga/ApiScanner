using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiScanner.DataAccess.Migrations
{
    public partial class AddWidgets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Widgets",
                columns: table => new
                {
                    WidgetId = table.Column<Guid>(nullable: false),
                    LocationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PublicRead = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Widgets", x => x.WidgetId);
                    table.ForeignKey(
                        name: "FK_Widgets_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Widgets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiWidgets",
                columns: table => new
                {
                    ApiWidgetId = table.Column<Guid>(nullable: false),
                    ApiId = table.Column<Guid>(nullable: false),
                    WidgetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiWidgets", x => x.ApiWidgetId);
                    table.ForeignKey(
                        name: "FK_ApiWidgets_Apis_ApiId",
                        column: x => x.ApiId,
                        principalTable: "Apis",
                        principalColumn: "ApiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiWidgets_Widgets_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "Widgets",
                        principalColumn: "WidgetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiWidgets_ApiId",
                table: "ApiWidgets",
                column: "ApiId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiWidgets_WidgetId",
                table: "ApiWidgets",
                column: "WidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_LocationId",
                table: "Widgets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_UserId",
                table: "Widgets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiWidgets");

            migrationBuilder.DropTable(
                name: "Widgets");
        }
    }
}
