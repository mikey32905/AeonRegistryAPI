using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeonRegistryAPI.Migrations
{
    /// <inheritdoc />
    public partial class _004UpdatedCatalogRecordModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "CatalogRecords",
                newName: "DateSubmitted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateSubmitted",
                table: "CatalogRecords",
                newName: "SubmittedAt");
        }
    }
}
