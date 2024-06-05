using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferenceValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterDataCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasterDataName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnumTypeId = table.Column<int>(type: "int", nullable: false),
                    MasterDataSymbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasterColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceValue", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferenceValue");
        }
    }
}
