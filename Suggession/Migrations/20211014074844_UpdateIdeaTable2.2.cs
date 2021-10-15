using Microsoft.EntityFrameworkCore.Migrations;

namespace Suggession.Migrations
{
    public partial class UpdateIdeaTable22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "IdeaHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "IdeaHistory");
        }
    }
}
