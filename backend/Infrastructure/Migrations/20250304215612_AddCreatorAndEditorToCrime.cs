using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorAndEditorToCrime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Crimes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditAt",
                table: "Crimes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EditorId",
                table: "Crimes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crimes_CreatorId",
                table: "Crimes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Crimes_EditorId",
                table: "Crimes",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crimes_Users_CreatorId",
                table: "Crimes",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Crimes_Users_EditorId",
                table: "Crimes",
                column: "EditorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crimes_Users_CreatorId",
                table: "Crimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Crimes_Users_EditorId",
                table: "Crimes");

            migrationBuilder.DropIndex(
                name: "IX_Crimes_CreatorId",
                table: "Crimes");

            migrationBuilder.DropIndex(
                name: "IX_Crimes_EditorId",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "EditAt",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Crimes");
        }
    }
}
