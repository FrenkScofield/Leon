using Microsoft.EntityFrameworkCore.Migrations;

namespace Leon.Migrations
{
    public partial class change_catagory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductCategories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "ProductCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "ProductCategories");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
