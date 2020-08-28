using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeServer.Data.Migrations
{
    public partial class createdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sdlc_system",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    base_url = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sdlc_system", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    external_id = table.Column<string>(nullable: true),
                    sdlc_systemid = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_project_sdlc_system_sdlc_systemid",
                        column: x => x.sdlc_systemid,
                        principalTable: "sdlc_system",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_project_sdlc_systemid",
                table: "project",
                column: "sdlc_systemid");

            migrationBuilder.CreateIndex(
                name: "IX_project_external_id_sdlc_systemid",
                table: "project",
                columns: new[] { "external_id", "sdlc_systemid" },
                unique: true,
                filter: "[external_id] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "sdlc_system");
        }
    }
}
