using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class columnsUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_RawMaterials_rawMaterialId",
                table: "RawMaterialRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_Recipes_recipeId",
                table: "RawMaterialRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterials_Recipes_Recipeid",
                table: "RawMaterials");

            migrationBuilder.DropIndex(
                name: "IX_RawMaterials_Recipeid",
                table: "RawMaterials");

            migrationBuilder.DropColumn(
                name: "Recipeid",
                table: "RawMaterials");

            migrationBuilder.RenameColumn(
                name: "recipeCode",
                table: "Recipes",
                newName: "RecipeCode");

            migrationBuilder.RenameColumn(
                name: "foodTypeId",
                table: "Recipes",
                newName: "FoodTypeId");

            migrationBuilder.RenameColumn(
                name: "addedDate",
                table: "Recipes",
                newName: "AddedDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Recipes",
                newName: "Id");

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

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_recipeId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_rawMaterialId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_RawMaterialId");

            migrationBuilder.AlterColumn<double>(
                name: "RawMaterialQuantity",
                table: "RawMaterialRecipe",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_RawMaterials_RawMaterialId",
                table: "RawMaterialRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterialRecipe_Recipes_RecipeId",
                table: "RawMaterialRecipe");

            migrationBuilder.RenameColumn(
                name: "RecipeCode",
                table: "Recipes",
                newName: "recipeCode");

            migrationBuilder.RenameColumn(
                name: "FoodTypeId",
                table: "Recipes",
                newName: "foodTypeId");

            migrationBuilder.RenameColumn(
                name: "AddedDate",
                table: "Recipes",
                newName: "addedDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recipes",
                newName: "id");

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

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_RecipeId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_recipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterialRecipe_RawMaterialId",
                table: "RawMaterialRecipe",
                newName: "IX_RawMaterialRecipe_rawMaterialId");

            migrationBuilder.AddColumn<int>(
                name: "Recipeid",
                table: "RawMaterials",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "rawMaterialQuantity",
                table: "RawMaterialRecipe",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterials_Recipeid",
                table: "RawMaterials",
                column: "Recipeid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterials_Recipes_Recipeid",
                table: "RawMaterials",
                column: "Recipeid",
                principalTable: "Recipes",
                principalColumn: "id");
        }
    }
}
