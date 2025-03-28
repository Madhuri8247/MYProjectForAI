using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineNews.Migrations
{
    /// <inheritdoc />
    public partial class dropisvisible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
