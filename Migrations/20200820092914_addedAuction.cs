using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class addedAuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
            migrationBuilder.CreateTable(
                name: "Auction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    image = table.Column<byte[]>(nullable: false),
                    startPrice = table.Column<float>(nullable: false),
                    openingDate = table.Column<DateTime>(nullable: false),
                    creationDate = table.Column<DateTime>(nullable: false),
                    closingDate = table.Column<DateTime>(nullable: false),
                    priceIncrement = table.Column<float>(nullable: false),
                    state = table.Column<string>(nullable: false),
                    userId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auction", x => x.id);
                    table.ForeignKey(
                        name: "FK_Auction_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           

            migrationBuilder.CreateIndex(
                name: "IX_Auction_userId",
                table: "Auction",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auction");


        }
    }
}
