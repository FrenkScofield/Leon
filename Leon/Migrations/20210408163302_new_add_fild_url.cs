using Microsoft.EntityFrameworkCore.Migrations;

namespace Leon.Migrations
{
    public partial class new_add_fild_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ProductCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "ProductCategories");
        }
    }
}
