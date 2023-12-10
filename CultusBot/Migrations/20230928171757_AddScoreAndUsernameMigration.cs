using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CultusBot.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreAndUsernameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonalScore",
                table: "ChatUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ChatUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalScore",
                table: "ChatUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ChatUsers");
        }
    }
}
