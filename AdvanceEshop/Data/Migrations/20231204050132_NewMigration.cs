using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvanceEshop.Data.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId1",
                table: "OrderDetails",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId1",
                table: "OrderDetails",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId1",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId1",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "OrderDetails");
        }
    }
}
