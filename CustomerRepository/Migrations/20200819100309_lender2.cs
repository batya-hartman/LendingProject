﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lender.Data.Migrations
{
    public partial class lender2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lenders",
                columns: table => new
                {
                    LenderId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PathToExcelFile = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lenders", x => x.LenderId);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LenderId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ComparisonOperator = table.Column<string>(nullable: true),
                    Operand = table.Column<string>(nullable: true),
                    LogicalOperator = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Lenders_LenderId",
                        column: x => x.LenderId,
                        principalTable: "Lenders",
                        principalColumn: "LenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rules_LenderId",
                table: "Rules",
                column: "LenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Lenders");
        }
    }
}
