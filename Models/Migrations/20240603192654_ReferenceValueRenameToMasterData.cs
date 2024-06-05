using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceValueRenameToMasterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReferenceValue",
                table: "ReferenceValue");

            migrationBuilder.RenameTable(
                name: "ReferenceValue",
                newName: "MasterData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterData",
                table: "MasterData",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterData",
                table: "MasterData");

            migrationBuilder.RenameTable(
                name: "MasterData",
                newName: "ReferenceValue");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReferenceValue",
                table: "ReferenceValue",
                column: "Id");
        }
    }
}
