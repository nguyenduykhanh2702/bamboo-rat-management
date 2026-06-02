using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BamBooRat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCageTransferTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CageTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    RatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromCageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ToCageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CageTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CageTransfers_Cages_FromCageId",
                        column: x => x.FromCageId,
                        principalTable: "Cages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CageTransfers_Cages_ToCageId",
                        column: x => x.ToCageId,
                        principalTable: "Cages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CageTransfers_Rats_RatId",
                        column: x => x.RatId,
                        principalTable: "Rats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CageTransfers_FromCageId",
                table: "CageTransfers",
                column: "FromCageId");

            migrationBuilder.CreateIndex(
                name: "IX_CageTransfers_RatId",
                table: "CageTransfers",
                column: "RatId");

            migrationBuilder.CreateIndex(
                name: "IX_CageTransfers_ToCageId",
                table: "CageTransfers",
                column: "ToCageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CageTransfers");
        }
    }
}
