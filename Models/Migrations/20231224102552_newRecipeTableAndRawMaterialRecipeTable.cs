using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class newRecipeTableAndRawMaterialRecipeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterials_FoodTypes_FoodTypeId",
                table: "RawMaterials");

            migrationBuilder.RenameColumn(
                name: "FoodTypeId",
                table: "RawMaterials",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterials_FoodTypeId",
                table: "RawMaterials",
                newName: "IX_RawMaterials_RecipeId");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    foodTypeId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawMaterialRecipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RawMaterialId = table.Column<int>(type: "int", nullable: false),
                    RawMaterialQuantity = table.Column<double>(type: "float", nullable: true),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterialRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawMaterialRecipe_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RawMaterialRecipe_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterialRecipe_RawMaterialId",
                table: "RawMaterialRecipe",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterialRecipe_RecipeId",
                table: "RawMaterialRecipe",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterials_Recipes_RecipeId",
                table: "RawMaterials",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterials_Recipes_RecipeId",
                table: "RawMaterials");

            migrationBuilder.DropTable(
                name: "RawMaterialRecipe");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RawMaterials",
                newName: "FoodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RawMaterials_RecipeId",
                table: "RawMaterials",
                newName: "IX_RawMaterials_FoodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterials_FoodTypes_FoodTypeId",
                table: "RawMaterials",
                column: "FoodTypeId",
                principalTable: "FoodTypes",
                principalColumn: "Id");
        }
    }
}
