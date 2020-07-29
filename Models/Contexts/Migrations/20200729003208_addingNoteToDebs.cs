using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP_SPARTAN.Data.Migrations
{
    public partial class addingNoteToDebs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Debs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Debs");
        }
    }
}
