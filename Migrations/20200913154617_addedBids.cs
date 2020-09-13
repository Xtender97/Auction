using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class addedBids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.CreateTable(
                name: "bid",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    auctionId = table.Column<int>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false),
                    oldPrice = table.Column<float>(nullable: false),
                    newPrice = table.Column<float>(nullable: false),
                    increment = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bid", x => x.id);
                    table.ForeignKey(
                        name: "FK_bid_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bid_auction_auctionId",
                        column: x => x.auctionId,
                        principalTable: "auction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });


            migrationBuilder.CreateIndex(
                name: "IX_bid_UserId",
                table: "bid",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_bid_auctionId",
                table: "bid",
                column: "auctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bid");

        }
    }
}
