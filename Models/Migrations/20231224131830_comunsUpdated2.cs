using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class comunsUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_RawMaterials_RawMaterialId",
                table: "RawMaterialRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_Recipes_RecipeId",
                table: "RawMaterialRecipe");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RawMaterialRecipe",
                newName: "recipeId");

            migrationBuilder.RenameColumn(
                name: "RawMaterialQuantity",
                table: "RawMaterialRecipe",
                newName: "rawMaterialQuantity");

            migrationBuilder.RenameColumn(
                name: "RawMaterialId",
                table: "RawMaterialRecipe",
                newName: "rawMaterialId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RawMaterialRecipe",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_RecipeId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_recipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_RawMaterialId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_rawMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterialRecipe_RawMaterials_rawMaterialId",
                table: "RawMaterialRecipe",
                column: "rawMaterialId",
                principalTable: "RawMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterialRecipe_Recipes_recipeId",
                table: "RawMaterialRecipe",
                column: "recipeId",
                principalTable: "Recipes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_RawMaterials_rawMaterialId",
                table: "RawMaterialRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_Recipes_recipeId",
                table: "RawMaterialRecipe");

            migrationBuilder.RenameColumn(
                name: "recipeId",
                table: "RawMaterialRecipe",
                newName: "RecipeId");

            migrationBuilder.RenameColumn(
                name: "rawMaterialQuantity",
                table: "RawMaterialRecipe",
                newName: "RawMaterialQuantity");

            migrationBuilder.RenameColumn(
                name: "rawMaterialId",
                table: "RawMaterialRecipe",
                newName: "RawMaterialId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RawMaterialRecipe",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_recipeId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_rawMaterialId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_RawMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterialRecipe_RawMaterials_RawMaterialId",
                table: "RawMaterialRecipe",
                column: "RawMaterialId",
                principalTable: "RawMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterialRecipe_Recipes_RecipeId",
                table: "RawMaterialRecipe",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
