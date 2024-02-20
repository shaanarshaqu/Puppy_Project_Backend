using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy_Project.Migrations
{
    /// <inheritdoc />
    public partial class imageprpertyadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profile_Photo",
                table: "UsersTb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile_Photo",
                table: "UsersTb");
        }
    }
}
