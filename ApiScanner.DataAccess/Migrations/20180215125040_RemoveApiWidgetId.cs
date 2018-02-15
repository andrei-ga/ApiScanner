using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiScanner.DataAccess.Migrations
{
    public partial class RemoveApiWidgetId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiWidgets",
                table: "ApiWidgets");

            migrationBuilder.DropIndex(
                name: "IX_ApiWidgets_ApiId",
                table: "ApiWidgets");

            migrationBuilder.DropColumn(
                name: "ApiWidgetId",
                table: "ApiWidgets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiWidgets",
                table: "ApiWidgets",
                columns: new[] { "ApiId", "WidgetId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiWidgets",
                table: "ApiWidgets");

            migrationBuilder.AddColumn<Guid>(
                name: "ApiWidgetId",
                table: "ApiWidgets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiWidgets",
                table: "ApiWidgets",
                column: "ApiWidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiWidgets_ApiId",
                table: "ApiWidgets",
                column: "ApiId");
        }
    }
}
