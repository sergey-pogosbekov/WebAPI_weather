using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Cities_cityInfoidCityKey",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "cityInfoidCityKey",
                table: "Favorites",
                newName: "cityInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_cityInfoidCityKey",
                table: "Favorites",
                newName: "IX_Favorites_cityInfoId");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "idFav",
            //    table: "Favorites",
            //    nullable: false,
            //    oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Cities_cityInfoId",
                table: "Favorites",
                column: "cityInfoId",
                principalTable: "Cities",
                principalColumn: "idCityKey",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Cities_cityInfoId",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "cityInfoId",
                table: "Favorites",
                newName: "cityInfoidCityKey");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_cityInfoId",
                table: "Favorites",
                newName: "IX_Favorites_cityInfoidCityKey");

            //migrationBuilder.AlterColumn<string>(
            //    name: "idFav",
            //    table: "Favorites",
            //    nullable: false,
            //    oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Cities_cityInfoidCityKey",
                table: "Favorites",
                column: "cityInfoidCityKey",
                principalTable: "Cities",
                principalColumn: "idCityKey",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
