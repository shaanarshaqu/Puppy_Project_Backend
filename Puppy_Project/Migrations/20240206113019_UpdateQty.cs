using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qty",
                table: "ProductsTb");

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "CartItemTb",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qty",
                table: "CartItemTb");

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "ProductsTb",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }
    }
}
