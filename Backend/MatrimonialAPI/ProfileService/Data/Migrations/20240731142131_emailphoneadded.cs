using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Migrations
{
    public partial class emailphoneadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BasicInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "BasicInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "BasicInfos");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "BasicInfos");
        }
    }
}
