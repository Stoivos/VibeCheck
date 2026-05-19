using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeCheck.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_places",
                columns: table => new
                {
                    pl_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pl_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pl_latitude = table.Column<double>(type: "float", nullable: false),
                    pl_longitude = table.Column<double>(type: "float", nullable: false),
                    pl_radius = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_places", x => x.pl_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_presence",
                columns: table => new
                {
                    pr_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pr_sessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pr_timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pr_place = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_presence", x => x.pr_id);
                    table.ForeignKey(
                        name: "FK_tbl_presence_tbl_places_pr_place",
                        column: x => x.pr_place,
                        principalTable: "tbl_places",
                        principalColumn: "pl_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_presence_pr_place",
                table: "tbl_presence",
                column: "pr_place");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_presence");

            migrationBuilder.DropTable(
                name: "tbl_places");
        }
    }
}
