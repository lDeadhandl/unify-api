using Microsoft.EntityFrameworkCore.Migrations;

namespace Unify.Data.Migrations
{
    public partial class UserUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "Uri");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyAccessToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyRefreshToken",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SpotifyAccessToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SpotifyRefreshToken",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "User",
                newName: "UserName");
        }
    }
}
