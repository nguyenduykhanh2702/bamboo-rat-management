using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BamBooRat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDeathRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeathRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    RatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeathDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cause = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeathRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeathRecords_Rats_RatId",
                        column: x => x.RatId,
                        principalTable: "Rats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeathRecords_RatId",
                table: "DeathRecords",
                column: "RatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeathRecords");
        }
    }
}
