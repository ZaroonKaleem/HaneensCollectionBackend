using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class StitchedSuitsApiAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StitchedSuitProductId",
                table: "ProductImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StitchedSuits",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shirt_EmbroideredNeckline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shirt_DigitalPrint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shirt_EmbroideredBorder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shirt_Fabric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shirt_Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dupatta_DigitalPrint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dupatta_Fabric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dupatta_Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trouser_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trouser_Fabric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trouser_Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePercentage = table.Column<int>(type: "int", nullable: true),
                    SizesAvailable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: true),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    ColorOptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    IsExclusive = table.Column<bool>(type: "bit", nullable: false),
                    ShippingInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StitchedSuits", x => x.ProductId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_StitchedSuitProductId",
                table: "ProductImages",
                column: "StitchedSuitProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_StitchedSuits_StitchedSuitProductId",
                table: "ProductImages",
                column: "StitchedSuitProductId",
                principalTable: "StitchedSuits",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_StitchedSuits_StitchedSuitProductId",
                table: "ProductImages");

            migrationBuilder.DropTable(
                name: "StitchedSuits");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_StitchedSuitProductId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "StitchedSuitProductId",
                table: "ProductImages");
        }
    }
}
