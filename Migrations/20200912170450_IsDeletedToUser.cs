using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class IsDeletedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auction_AspNetUsers_userId",
                table: "Auction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auction",
                table: "Auction");


            migrationBuilder.RenameTable(
                name: "Auction",
                newName: "auction");

            migrationBuilder.RenameIndex(
                name: "IX_Auction_userId",
                table: "auction",
                newName: "IX_auction_userId");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_auction",
                table: "auction",
                column: "id");
          

            migrationBuilder.AddForeignKey(
                name: "FK_auction_AspNetUsers_userId",
                table: "auction",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auction_AspNetUsers_userId",
                table: "auction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auction",
                table: "auction");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86d95767-3319-46ff-91bb-736041d80a5b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b30c8d31-a257-4802-bdc1-13c45f2eda3e");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "auction",
                newName: "Auction");

            migrationBuilder.RenameIndex(
                name: "IX_auction_userId",
                table: "Auction",
                newName: "IX_Auction_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auction",
                table: "Auction",
                column: "id");


            migrationBuilder.AddForeignKey(
                name: "FK_Auction_AspNetUsers_userId",
                table: "Auction",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
