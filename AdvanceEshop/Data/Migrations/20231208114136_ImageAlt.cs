using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvanceEshop.Data.Migrations
{
    public partial class ImageAlt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageAlt",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAlt",
                table: "Products");
        }
    }
}
