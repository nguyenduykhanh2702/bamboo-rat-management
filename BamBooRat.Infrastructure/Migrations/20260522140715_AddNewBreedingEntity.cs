using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BamBooRat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewBreedingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Rats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Breedings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FemaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalFemaleCageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BreedingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedBirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualBirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OffspringCount = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breedings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breedings_Cages_CageId",
                        column: x => x.CageId,
                        principalTable: "Cages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Breedings_Rats_FemaleId",
                        column: x => x.FemaleId,
                        principalTable: "Rats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Breedings_Rats_MaleId",
                        column: x => x.MaleId,
                        principalTable: "Rats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Breedings_CageId",
                table: "Breedings",
                column: "CageId");

            migrationBuilder.CreateIndex(
                name: "IX_Breedings_FemaleId",
                table: "Breedings",
                column: "FemaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Breedings_MaleId",
                table: "Breedings",
                column: "MaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breedings");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Rats");
        }
    }
}
