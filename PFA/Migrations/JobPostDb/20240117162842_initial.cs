using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFA.Migrations.JobPostDb
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobPosts",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    applylink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    txtDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    inputDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    companyImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPosts", x => x.JobID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobPosts");
        }
    }
}
