using Microsoft.EntityFrameworkCore.Migrations;

namespace Unify.Data.Migrations
{
    public partial class FixPartyIdOnUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Party_User_UserId",
                table: "Party");

            migrationBuilder.DropIndex(
                name: "IX_Party_UserId",
                table: "Party");

            migrationBuilder.AlterColumn<int>(
                name: "PartyId",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartyId1",
                table: "User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Party",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PartyId1",
                table: "User",
                column: "PartyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Party_PartyId1",
                table: "User",
                column: "PartyId1",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Party_PartyId1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PartyId1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PartyId1",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "PartyId",
                table: "User",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Party",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Party_UserId",
                table: "Party",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Party_User_UserId",
                table: "Party",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
