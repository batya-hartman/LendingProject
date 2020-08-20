using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendings.Data.Migrations
{
    public partial class lending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "LendingEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LenderId = table.Column<Guid>(nullable: false),
                    PrincipalSignature = table.Column<string>(nullable: true),
                    Confirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendingEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendingEntities_Lenders_LenderId",
                        column: x => x.LenderId,
                        principalTable: "Lenders",
                        principalColumn: "LenderId",
                        onDelete: ReferentialAction.Cascade);
                });

           
                

            migrationBuilder.CreateTable(
                name: "Parameter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LendingId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    LendingEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameter_LendingEntities_LendingEntityId",
                        column: x => x.LendingEntityId,
                        principalTable: "LendingEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LendingEntities_LenderId",
                table: "LendingEntities",
                column: "LenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_LendingEntityId",
                table: "Parameter",
                column: "LendingEntityId");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "LendingEntities");

        
        }
    }
}
