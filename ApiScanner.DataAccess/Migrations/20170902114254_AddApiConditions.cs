using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ApiScanner.DataAccess.Migrations
{
    public partial class AddApiConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    ConditionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompareType = table.Column<int>(type: "int4", nullable: false),
                    CompareValue = table.Column<string>(type: "text", nullable: true),
                    MatchType = table.Column<int>(type: "int4", nullable: false),
                    MatchVar = table.Column<string>(type: "text", nullable: true),
                    ShouldPass = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.ConditionId);
                    table.ForeignKey(
                        name: "FK_Conditions_Apis_ApiId",
                        column: x => x.ApiId,
                        principalTable: "Apis",
                        principalColumn: "ApiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_ApiId",
                table: "Conditions",
                column: "ApiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conditions");
        }
    }
}
