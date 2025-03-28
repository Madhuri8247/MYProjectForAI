using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineNews.Migrations
{
    /// <inheritdoc />
    public partial class addUserInteractionwithArticleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsApproved",
            //    table: "Articles",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserInteractionWithArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Liked = table.Column<bool>(type: "bit", nullable: false),
                    Disliked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteractionWithArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInteractionWithArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInteractionWithArticles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractionWithArticles_ArticleId",
                table: "UserInteractionWithArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractionWithArticles_UserId",
                table: "UserInteractionWithArticles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInteractionWithArticles");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Articles");
        }
    }
}
