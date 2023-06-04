using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieScribe.Migrations
{
    /// <inheritdoc />
    public partial class PTW2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanToWatch_AspNetUsers_AppUserId",
                table: "PlanToWatch");

            migrationBuilder.DropIndex(
                name: "IX_PlanToWatch_AppUserId",
                table: "PlanToWatch");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "PlanToWatch");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "PlanToWatch",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PlanToWatch",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "PlanToWatch",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PlanToWatch",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "PlanToWatch",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanToWatch_AppUserId",
                table: "PlanToWatch",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanToWatch_AspNetUsers_AppUserId",
                table: "PlanToWatch",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
