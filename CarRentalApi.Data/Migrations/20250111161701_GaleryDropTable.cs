using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class GaleryDropTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Gallerys_GalleryId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Gallerys");

            migrationBuilder.DropIndex(
                name: "IX_Cars_GalleryId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "Cars");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GalleryId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Gallerys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallerys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_GalleryId",
                table: "Cars",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Gallerys_GalleryId",
                table: "Cars",
                column: "GalleryId",
                principalTable: "Gallerys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
