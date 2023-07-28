using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Institute.Migrations
{
    public partial class Helper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Novel",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Novel",
                table: "Students");
        }
    }
}
