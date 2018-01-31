using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiScanner.DataAccess.Migrations
{
    public partial class AddApiLogLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "ApiLogs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ApiLogs_LocationId",
                table: "ApiLogs",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiLogs_Locations_LocationId",
                table: "ApiLogs",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiLogs_Locations_LocationId",
                table: "ApiLogs");

            migrationBuilder.DropIndex(
                name: "IX_ApiLogs_LocationId",
                table: "ApiLogs");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ApiLogs");
        }
    }
}
