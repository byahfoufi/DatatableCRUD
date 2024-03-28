using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatatableCRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddFileNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "MediaItems");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "MediaItems",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "MediaItems",
                newName: "FilePath");

            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "MediaItems",
                type: "int",
                nullable: true);
        }
    }
}
