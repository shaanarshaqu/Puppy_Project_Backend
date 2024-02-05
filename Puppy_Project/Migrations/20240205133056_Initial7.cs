using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy_Project.Migrations
{
    /// <inheritdoc />
    public partial class Initial7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItemTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cart_id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItemTb_CartTb_Cart_id",
                        column: x => x.Cart_id,
                        principalTable: "CartTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemTb_ProductsTb_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "ProductsTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemTb_Cart_id",
                table: "CartItemTb",
                column: "Cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemTb_Product_Id",
                table: "CartItemTb",
                column: "Product_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemTb");
        }
    }
}
