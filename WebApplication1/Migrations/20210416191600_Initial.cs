using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    idCityKey = table.Column<string>(nullable: false),
                    idCityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.idCityKey);
                });

            migrationBuilder.CreateTable(
                name: "WeathersDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cityIdAndKeyidCityKey = table.Column<string>(nullable: true),
                    metricValue = table.Column<double>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    dateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeathersDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeathersDetails_Cities_cityIdAndKeyidCityKey",
                        column: x => x.cityIdAndKeyidCityKey,
                        principalTable: "Cities",
                        principalColumn: "idCityKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeathersDetails_cityIdAndKeyidCityKey",
                table: "WeathersDetails",
                column: "cityIdAndKeyidCityKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeathersDetails");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
