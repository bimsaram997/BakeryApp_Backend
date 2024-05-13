using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class LocationIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "RawMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BatchProduct_ProductId",
                table: "BatchProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchProduct_Product_ProductId",
                table: "BatchProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchProduct_Product_ProductId",
                table: "BatchProduct");

            migrationBuilder.DropIndex(
                name: "IX_BatchProduct_ProductId",
                table: "BatchProduct");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "RawMaterials");
        }
    }
}
