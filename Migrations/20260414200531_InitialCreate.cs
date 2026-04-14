using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Longfunctie.api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PinHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "children",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AnonymousId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_children_Parents_ParentName",
                        column: x => x.ParentName,
                        principalTable: "Parents",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_children_ParentName",
                table: "children",
                column: "ParentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "children");

            migrationBuilder.DropTable(
                name: "Parents");
        }
    }
}
