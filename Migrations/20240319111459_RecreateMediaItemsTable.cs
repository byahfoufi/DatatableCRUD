using Microsoft.EntityFrameworkCore.Migrations;

namespace DatatableCRUD.Migrations
{
    public partial class RecreateMediaItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    MediaItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"), // Primary Key
                    Title = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true), // Optional Link
                    FileName = table.Column<string>(nullable: true), // Optional FileName
                    MediaType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.MediaItemId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MediaItems");
        }
    }
}
