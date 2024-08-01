using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Migrations
{
    public partial class partnerprefgallaryimages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartnerPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinHeight = table.Column<int>(type: "int", nullable: false),
                    MaxHeight = table.Column<int>(type: "int", nullable: false),
                    MinWeight = table.Column<int>(type: "int", nullable: false),
                    MaxWeight = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimumQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmokeAcceptable = table.Column<bool>(type: "bit", nullable: false),
                    DrinkAcceptable = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complexion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileImages_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_PartnerPreId",
                table: "UserProfiles",
                column: "PartnerPreId",
                unique: true,
                filter: "[PartnerPreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserProfileId",
                table: "ProfileImages",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_PartnerPreferences_PartnerPreId",
                table: "UserProfiles",
                column: "PartnerPreId",
                principalTable: "PartnerPreferences",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_PartnerPreferences_PartnerPreId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "PartnerPreferences");

            migrationBuilder.DropTable(
                name: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_PartnerPreId",
                table: "UserProfiles");
        }
    }
}
