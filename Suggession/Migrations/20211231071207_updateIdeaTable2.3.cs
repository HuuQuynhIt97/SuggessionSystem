using Microsoft.EntityFrameworkCore.Migrations;

namespace Suggession.Migrations
{
    public partial class updateIdeaTable23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Tab",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameZh",
                table: "Tab",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Tab",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Status",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameZh",
                table: "Status",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentZh",
                table: "IdeaHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnnouncement",
                table: "Idea",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SystemLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SLPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SLType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SLKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SLTW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SLEN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLanguages", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemLanguages");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Tab");

            migrationBuilder.DropColumn(
                name: "NameZh",
                table: "Tab");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tab");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "NameZh",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "CommentZh",
                table: "IdeaHistory");

            migrationBuilder.DropColumn(
                name: "IsAnnouncement",
                table: "Idea");
        }
    }
}
