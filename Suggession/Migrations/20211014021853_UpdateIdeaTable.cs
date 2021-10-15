using Microsoft.EntityFrameworkCore.Migrations;

namespace Suggession.Migrations
{
    public partial class UpdateIdeaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Issue",
                table: "Idea",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Issue",
                table: "Idea");
        }
    }
}
