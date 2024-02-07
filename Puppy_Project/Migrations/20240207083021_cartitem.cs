using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy_Project.Migrations
{
    /// <inheritdoc />
    public partial class cartitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTb_ProductsTb_ProductId",
                table: "OrderTb");

            migrationBuilder.DropIndex(
                name: "IX_OrderTb_ProductId",
                table: "OrderTb");

            migrationBuilder.DropIndex(
                name: "IX_OrderTb_User_Id",
                table: "OrderTb");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderTb");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "OrderTb");

            migrationBuilder.AddColumn<int>(
                name: "ProductDTOId",
                table: "OrderTb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItemTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Id = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemTb_OrderTb_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "OrderTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemTb_ProductsTb_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductsTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_ProductDTOId",
                table: "OrderTb",
                column: "ProductDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_User_Id",
                table: "OrderTb",
                column: "User_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTb_Order_Id",
                table: "OrderItemTb",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTb_ProductId",
                table: "OrderItemTb",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTb_ProductsTb_ProductDTOId",
                table: "OrderTb",
                column: "ProductDTOId",
                principalTable: "ProductsTb",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTb_ProductsTb_ProductDTOId",
                table: "OrderTb");

            migrationBuilder.DropTable(
                name: "OrderItemTb");

            migrationBuilder.DropIndex(
                name: "IX_OrderTb_ProductDTOId",
                table: "OrderTb");

            migrationBuilder.DropIndex(
                name: "IX_OrderTb_User_Id",
                table: "OrderTb");

            migrationBuilder.DropColumn(
                name: "ProductDTOId",
                table: "OrderTb");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderTb",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "OrderTb",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_ProductId",
                table: "OrderTb",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_User_Id",
                table: "OrderTb",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTb_ProductsTb_ProductId",
                table: "OrderTb",
                column: "ProductId",
                principalTable: "ProductsTb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
