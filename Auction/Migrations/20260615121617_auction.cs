using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction.Migrations
{
    /// <inheritdoc />
    public partial class auction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "Varchar(40)", nullable: false),
                    ProductName = table.Column<string>(type: "Varchar(40)", nullable: false),
                    Description = table.Column<string>(type: "Varchar(100)", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    currentBid = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ImageUrl = table.Column<string>(type: "Varchar(50)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "Varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionID);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "Varchar(40)", nullable: false),
                    AuctionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_Images_Auctions_AuctionID",
                        column: x => x.AuctionID,
                        principalTable: "Auctions",
                        principalColumn: "AuctionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_AuctionID",
                table: "Images",
                column: "AuctionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Auctions");
        }
    }
}
