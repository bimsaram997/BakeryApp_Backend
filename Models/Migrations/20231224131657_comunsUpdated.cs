using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class comunsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterials_Recipes_RecipeId",
                table: "RawMaterials");

            migrationBuilder.RenameColumn(
                name: "RecipeCode",
                table: "Recipes",
                newName: "recipeCode");

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
                table: "RawMaterials",
                newName: "Recipeid");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterials_RecipeId",
                table: "RawMaterials",
                newName: "IX_RawMaterials_Recipeid");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterials_Recipes_Recipeid",
                table: "RawMaterials",
                column: "Recipeid",
                principalTable: "Recipes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterials_Recipes_Recipeid",
                table: "RawMaterials");

            migrationBuilder.RenameColumn(
                name: "recipeCode",
                table: "Recipes",
                newName: "RecipeCode");

            migrationBuilder.RenameColumn(
                name: "addedDate",
                table: "Recipes",
                newName: "AddedDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Recipes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Recipeid",
                table: "RawMaterials",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterials_Recipeid",
                table: "RawMaterials",
                newName: "IX_RawMaterials_RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterials_Recipes_RecipeId",
                table: "RawMaterials",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
