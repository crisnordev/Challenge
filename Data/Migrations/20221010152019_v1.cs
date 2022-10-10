using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAppChallenge.Data.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CourseTitle = table.Column<string>(type: "NVARCHAR", maxLength: 80, nullable: false),
                    Tag = table.Column<string>(type: "NVARCHAR", maxLength: 4, nullable: false),
                    Summary = table.Column<string>(type: "NVARCHAR", maxLength: 160, nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    CourseItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CourseItemTitle = table.Column<string>(type: "NVARCHAR", maxLength: 80, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.CourseItemId);
                    table.ForeignKey(
                        name: "FK_CourseItem_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecture",
                columns: table => new
                {
                    LectureId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LectureTitle = table.Column<string>(type: "NVARCHAR", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR", maxLength: 160, nullable: false),
                    VideoUrl = table.Column<string>(type: "NVARCHAR", maxLength: 2046, nullable: false),
                    CourseItemId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecture", x => x.LectureId);
                    table.ForeignKey(
                        name: "FK_Lecture_CourseItemId",
                        column: x => x.CourseItemId,
                        principalTable: "Module",
                        principalColumn: "CourseItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_CourseItemId",
                table: "Lecture",
                column: "CourseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_CourseId",
                table: "Module",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lecture");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
