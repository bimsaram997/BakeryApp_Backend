using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class addBatcIDToFoodItemh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BatchFoodItem_FoodItemId",
                table: "BatchFoodItem",
                column: "FoodItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchFoodItem_FoodItems_FoodItemId",
                table: "BatchFoodItem",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchFoodItem_FoodItems_FoodItemId",
                table: "BatchFoodItem");

            migrationBuilder.DropIndex(
                name: "IX_BatchFoodItem_FoodItemId",
                table: "BatchFoodItem");
        }
    }
}
