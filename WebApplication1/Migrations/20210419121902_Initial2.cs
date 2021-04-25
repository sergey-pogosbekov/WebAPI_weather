using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebApplication1.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    idFav = table.Column<Guid>(nullable: false),
                    cityInfoidCityKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.idFav);
                    table.ForeignKey(
                        name: "FK_Favorites_Cities_cityInfoidCityKey",
                        column: x => x.cityInfoidCityKey,
                        principalTable: "Cities",
                        principalColumn: "idCityKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_cityInfoidCityKey",
                table: "Favorites",
                column: "cityInfoidCityKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");
        }
    }
}
