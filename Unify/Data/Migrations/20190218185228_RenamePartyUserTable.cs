using Microsoft.EntityFrameworkCore.Migrations;

namespace Unify.Data.Migrations
{
    public partial class RenamePartyUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyUser_Party_PartyId",
                table: "PartyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyUser_User_UserId",
                table: "PartyUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartyUser",
                table: "PartyUser");

            migrationBuilder.RenameTable(
                name: "PartyUser",
                newName: "Guests");

            migrationBuilder.RenameIndex(
                name: "IX_PartyUser_UserId",
                table: "Guests",
                newName: "IX_Guests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PartyUser_PartyId",
                table: "Guests",
                newName: "IX_Guests_PartyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guests",
                table: "Guests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Party_PartyId",
                table: "Guests",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_User_UserId",
                table: "Guests",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Party_PartyId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Guests_User_UserId",
                table: "Guests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guests",
                table: "Guests");

            migrationBuilder.RenameTable(
                name: "Guests",
                newName: "PartyUser");

            migrationBuilder.RenameIndex(
                name: "IX_Guests_UserId",
                table: "PartyUser",
                newName: "IX_PartyUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Guests_PartyId",
                table: "PartyUser",
                newName: "IX_PartyUser_PartyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartyUser",
                table: "PartyUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyUser_Party_PartyId",
                table: "PartyUser",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyUser_User_UserId",
                table: "PartyUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
