using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiScanner.DataAccess.Migrations
{
    public partial class ApiLogChangeResponseTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ResponseTime",
                table: "ApiLogs",
                type: "int8",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ResponseTime",
                table: "ApiLogs",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "int8");
        }
    }
}
