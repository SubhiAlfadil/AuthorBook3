using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthorBook3.Migrations
{
    public partial class BookUpdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bookFile",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bookFile",
                table: "Books");
        }
    }
}
