using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieRatingBackend.Migrations
{
    public partial class addedratingsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RatingNum = table.Column<int>(nullable: false),
                    MediaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Medias_MediaID",
                        column: x => x.MediaID,
                        principalTable: "Medias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MediaID",
                table: "Rating",
                column: "MediaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
