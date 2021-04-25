using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeathersDetails_Cities_cityIdAndKeyidCityKey",
                table: "WeathersDetails");

            migrationBuilder.RenameColumn(
                name: "cityIdAndKeyidCityKey",
                table: "WeathersDetails",
                newName: "cityInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_WeathersDetails_cityIdAndKeyidCityKey",
                table: "WeathersDetails",
                newName: "IX_WeathersDetails_cityInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeathersDetails_Cities_cityInfoId",
                table: "WeathersDetails",
                column: "cityInfoId",
                principalTable: "Cities",
                principalColumn: "idCityKey",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeathersDetails_Cities_cityInfoId",
                table: "WeathersDetails");

            migrationBuilder.RenameColumn(
                name: "cityInfoId",
                table: "WeathersDetails",
                newName: "cityIdAndKeyidCityKey");

            migrationBuilder.RenameIndex(
                name: "IX_WeathersDetails_cityInfoId",
                table: "WeathersDetails",
                newName: "IX_WeathersDetails_cityIdAndKeyidCityKey");

            migrationBuilder.AddForeignKey(
                name: "FK_WeathersDetails_Cities_cityIdAndKeyidCityKey",
                table: "WeathersDetails",
                column: "cityIdAndKeyidCityKey",
                principalTable: "Cities",
                principalColumn: "idCityKey",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
