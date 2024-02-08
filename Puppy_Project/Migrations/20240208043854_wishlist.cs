using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy_Project.Migrations
{
    /// <inheritdoc />
    public partial class wishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WishListTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishListTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishListTb_UsersTb_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishListItemTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WishList_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishListItemTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishListItemTb_ProductsTb_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "ProductsTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishListItemTb_WishListTb_WishList_Id",
                        column: x => x.WishList_Id,
                        principalTable: "WishListTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishListItemTb_Product_Id",
                table: "WishListItemTb",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WishListItemTb_WishList_Id",
                table: "WishListItemTb",
                column: "WishList_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WishListTb_UserId",
                table: "WishListTb",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishListItemTb");

            migrationBuilder.DropTable(
                name: "WishListTb");
        }
    }
}
