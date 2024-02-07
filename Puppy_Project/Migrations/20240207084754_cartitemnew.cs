using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy_Project.Migrations
{
    /// <inheritdoc />
    public partial class cartitemnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemTb_ProductsTb_ProductId",
                table: "OrderItemTb");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTb_ProductsTb_ProductDTOId",
                table: "OrderTb");

            migrationBuilder.DropIndex(
                name: "IX_OrderTb_ProductDTOId",
                table: "OrderTb");

            migrationBuilder.DropColumn(
                name: "ProductDTOId",
                table: "OrderTb");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItemTb",
                newName: "Product_Id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemTb_ProductId",
                table: "OrderItemTb",
                newName: "IX_OrderItemTb_Product_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemTb_ProductsTb_Product_Id",
                table: "OrderItemTb",
                column: "Product_Id",
                principalTable: "ProductsTb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemTb_ProductsTb_Product_Id",
                table: "OrderItemTb");

            migrationBuilder.RenameColumn(
                name: "Product_Id",
                table: "OrderItemTb",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemTb_Product_Id",
                table: "OrderItemTb",
                newName: "IX_OrderItemTb_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "ProductDTOId",
                table: "OrderTb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_ProductDTOId",
                table: "OrderTb",
                column: "ProductDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemTb_ProductsTb_ProductId",
                table: "OrderItemTb",
                column: "ProductId",
                principalTable: "ProductsTb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTb_ProductsTb_ProductDTOId",
                table: "OrderTb",
                column: "ProductDTOId",
                principalTable: "ProductsTb",
                principalColumn: "Id");
        }
    }
}
