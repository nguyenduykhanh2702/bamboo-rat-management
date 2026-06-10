using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BamBooRat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewHealthRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WeightHistories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WeightHistories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CageTransfers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CageTransfers");

            migrationBuilder.CreateTable(
                name: "HealthRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    RatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Treatment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Medicine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthRecords_Rats_RatId",
                        column: x => x.RatId,
                        principalTable: "Rats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_RatId",
                table: "HealthRecords",
                column: "RatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthRecords");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WeightHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WeightHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CageTransfers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CageTransfers",
                type: "datetime2",
                nullable: true);
        }
    }
}
