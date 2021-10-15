using Microsoft.EntityFrameworkCore.Migrations;

namespace Suggession.Migrations
{
    public partial class UpdateIdeaTable21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "IdeaHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "Isshow",
                table: "IdeaHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Isshow",
                table: "Idea",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isshow",
                table: "IdeaHistory");

            migrationBuilder.DropColumn(
                name: "Isshow",
                table: "Idea");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "IdeaHistory",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
