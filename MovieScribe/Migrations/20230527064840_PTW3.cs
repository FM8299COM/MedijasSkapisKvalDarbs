using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieScribe.Migrations
{
    /// <inheritdoc />
    public partial class PTW3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaModelID",
                table: "PlanToWatch",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Watched",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediaId = table.Column<int>(type: "int", nullable: false),
                    MediaModelID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watched", x => new { x.UserId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_Watched_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Watched_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Watched_Media_MediaModelID",
                        column: x => x.MediaModelID,
                        principalTable: "Media",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanToWatch_MediaModelID",
                table: "PlanToWatch",
                column: "MediaModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Watched_MediaId",
                table: "Watched",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Watched_MediaModelID",
                table: "Watched",
                column: "MediaModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanToWatch_Media_MediaModelID",
                table: "PlanToWatch",
                column: "MediaModelID",
                principalTable: "Media",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanToWatch_Media_MediaModelID",
                table: "PlanToWatch");

            migrationBuilder.DropTable(
                name: "Watched");

            migrationBuilder.DropIndex(
                name: "IX_PlanToWatch_MediaModelID",
                table: "PlanToWatch");

            migrationBuilder.DropColumn(
                name: "MediaModelID",
                table: "PlanToWatch");
        }
    }
}
