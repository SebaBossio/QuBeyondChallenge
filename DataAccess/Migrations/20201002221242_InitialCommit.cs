using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Searches",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    CreatedBy = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    CTS = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ModifyBy = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    MTS = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UserName = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    AlgorithmKey = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    WordStream = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    Matrix = table.Column<string>(type: "VARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Searches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Searches",
                schema: "dbo");
        }
    }
}
