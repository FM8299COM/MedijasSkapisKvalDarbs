using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieScribe.Migrations
{
    /// <inheritdoc />
    public partial class PTW6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanToWatch_Media_MediaModelID",
                table: "PlanToWatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Watched_Media_MediaModelID",
                table: "Watched");

            migrationBuilder.DropIndex(
                name: "IX_Watched_MediaModelID",
                table: "Watched");

            migrationBuilder.DropIndex(
                name: "IX_PlanToWatch_MediaModelID",
                table: "PlanToWatch");

            migrationBuilder.DropColumn(
                name: "MediaModelID",
                table: "Watched");

            migrationBuilder.DropColumn(
                name: "MediaModelID",
                table: "PlanToWatch");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaModelID",
                table: "Watched",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaModelID",
                table: "PlanToWatch",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Watched_MediaModelID",
                table: "Watched",
                column: "MediaModelID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanToWatch_MediaModelID",
                table: "PlanToWatch",
                column: "MediaModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanToWatch_Media_MediaModelID",
                table: "PlanToWatch",
                column: "MediaModelID",
                principalTable: "Media",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Watched_Media_MediaModelID",
                table: "Watched",
                column: "MediaModelID",
                principalTable: "Media",
                principalColumn: "ID");
        }
    }
}
