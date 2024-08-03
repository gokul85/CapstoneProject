using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace ProfileService.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class othermodelsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HighestQualification",
                table: "BasicInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PinCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Careers_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Father = table.Column<bool>(type: "bit", nullable: false),
                    Mother = table.Column<bool>(type: "bit", nullable: false),
                    Siblings = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lifestyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Drink = table.Column<bool>(type: "bit", nullable: false),
                    Smoke = table.Column<bool>(type: "bit", nullable: false),
                    LivingWith = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lifestyles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    EyeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HairColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complextion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disability = table.Column<bool>(type: "bit", nullable: false),
                    DisablitiyDetail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalAttributes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AddressId",
                table: "UserProfiles",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_FamilyInfoId",
                table: "UserProfiles",
                column: "FamilyInfoId",
                unique: true,
                filter: "[FamilyInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_LifeStyleId",
                table: "UserProfiles",
                column: "LifeStyleId",
                unique: true,
                filter: "[LifeStyleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_PhysicalAttrId",
                table: "UserProfiles",
                column: "PhysicalAttrId",
                unique: true,
                filter: "[PhysicalAttrId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Careers_UserProfileId",
                table: "Careers",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UserProfileId",
                table: "Educations",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Addresses_AddressId",
                table: "UserProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_FamilyInfos_FamilyInfoId",
                table: "UserProfiles",
                column: "FamilyInfoId",
                principalTable: "FamilyInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Lifestyles_LifeStyleId",
                table: "UserProfiles",
                column: "LifeStyleId",
                principalTable: "Lifestyles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_PhysicalAttributes_PhysicalAttrId",
                table: "UserProfiles",
                column: "PhysicalAttrId",
                principalTable: "PhysicalAttributes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Addresses_AddressId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_FamilyInfos_FamilyInfoId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Lifestyles_LifeStyleId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_PhysicalAttributes_PhysicalAttrId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Careers");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "FamilyInfos");

            migrationBuilder.DropTable(
                name: "Lifestyles");

            migrationBuilder.DropTable(
                name: "PhysicalAttributes");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AddressId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_FamilyInfoId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_LifeStyleId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_PhysicalAttrId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "HighestQualification",
                table: "BasicInfos");
        }
    }
}
