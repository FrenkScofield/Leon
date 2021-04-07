using Microsoft.EntityFrameworkCore.Migrations;

namespace Leon.Migrations
{
    public partial class add_new_fild_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Products");
        }
    }
}
