using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BamBooRat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitBreedingClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breedings_Cages_CageId",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "OriginalFemaleCageId",
                table: "Breedings");

            migrationBuilder.RenameColumn(
                name: "BreedingDate",
                table: "Breedings",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeparationDate",
                table: "Rats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Cages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpectedBirthDate",
                table: "Breedings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualSeparationDate",
                table: "Breedings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BreedingStatus",
                table: "Breedings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedSeparationDate",
                table: "Breedings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "FemaleOldCageId",
                table: "Breedings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsBirthSuccessful",
                table: "Breedings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOffspringSurvived",
                table: "Breedings",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Breedings_FemaleOldCageId",
                table: "Breedings",
                column: "FemaleOldCageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Breedings_Cages_CageId",
                table: "Breedings",
                column: "CageId",
                principalTable: "Cages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Breedings_Cages_FemaleOldCageId",
                table: "Breedings",
                column: "FemaleOldCageId",
                principalTable: "Cages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breedings_Cages_CageId",
                table: "Breedings");

            migrationBuilder.DropForeignKey(
                name: "FK_Breedings_Cages_FemaleOldCageId",
                table: "Breedings");

            migrationBuilder.DropIndex(
                name: "IX_Breedings_FemaleOldCageId",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "LastSeparationDate",
                table: "Rats");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Cages");

            migrationBuilder.DropColumn(
                name: "ActualSeparationDate",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "BreedingStatus",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "ExpectedSeparationDate",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "FemaleOldCageId",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "IsBirthSuccessful",
                table: "Breedings");

            migrationBuilder.DropColumn(
                name: "IsOffspringSurvived",
                table: "Breedings");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Breedings",
                newName: "BreedingDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpectedBirthDate",
                table: "Breedings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "OriginalFemaleCageId",
                table: "Breedings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Breedings_Cages_CageId",
                table: "Breedings",
                column: "CageId",
                principalTable: "Cages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
