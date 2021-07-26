using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieRatingBackend.Migrations
{
    public partial class updatemodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Medias_MediaID",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_MediaID",
                table: "Ratings",
                newName: "IX_Ratings_MediaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Medias_MediaID",
                table: "Ratings",
                column: "MediaID",
                principalTable: "Medias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Medias_MediaID",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_MediaID",
                table: "Rating",
                newName: "IX_Rating_MediaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Medias_MediaID",
                table: "Rating",
                column: "MediaID",
                principalTable: "Medias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
