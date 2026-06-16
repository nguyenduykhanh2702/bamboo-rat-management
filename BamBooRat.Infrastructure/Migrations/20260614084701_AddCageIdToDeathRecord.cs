using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BamBooRat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCageIdToDeathRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CageId",
                table: "DeathRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeathRecords_CageId",
                table: "DeathRecords",
                column: "CageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeathRecords_Cages_CageId",
                table: "DeathRecords",
                column: "CageId",
                principalTable: "Cages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeathRecords_Cages_CageId",
                table: "DeathRecords");

            migrationBuilder.DropIndex(
                name: "IX_DeathRecords_CageId",
                table: "DeathRecords");

            migrationBuilder.DropColumn(
                name: "CageId",
                table: "DeathRecords");
        }
    }
}
