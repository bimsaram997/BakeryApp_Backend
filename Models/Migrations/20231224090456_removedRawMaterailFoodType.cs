using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class removedRawMaterailFoodType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RawMaterial_FoodTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RawMaterial_FoodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodTypeId = table.Column<int>(type: "int", nullable: false),
                    RawMaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterial_FoodTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawMaterial_FoodTypes_FoodTypes_FoodTypeId",
                        column: x => x.FoodTypeId,
                        principalTable: "FoodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RawMaterial_FoodTypes_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterial_FoodTypes_FoodTypeId",
                table: "RawMaterial_FoodTypes",
                column: "FoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterial_FoodTypes_RawMaterialId",
                table: "RawMaterial_FoodTypes",
                column: "RawMaterialId");
        }
    }
}
