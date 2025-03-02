using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLawsuit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Crimes_Lawsuits_LawsuitId",
            //    table: "Crimes");

            //migrationBuilder.DropTable(
            //    name: "Lawsuits");

            //migrationBuilder.DropIndex(
            //    name: "IX_Crimes_LawsuitId",
            //    table: "Crimes");

            //migrationBuilder.DropColumn(
            //    name: "LawsuitId",
            //    table: "Crimes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LawsuitId",
                table: "Crimes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lawsuits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Decision = table.Column<string>(type: "text", nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Judge = table.Column<string>(type: "text", nullable: false),
                    JudicialActs = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lawsuits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lawsuits_WantedPersons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "WantedPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Crimes_LawsuitId",
                table: "Crimes",
                column: "LawsuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Lawsuits_PersonId",
                table: "Lawsuits",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crimes_Lawsuits_LawsuitId",
                table: "Crimes",
                column: "LawsuitId",
                principalTable: "Lawsuits",
                principalColumn: "Id");
        }
    }
}
