using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finaktiva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableExceptionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Json",
                table: "ExcepcionLogs",
                newName: "StackTrace");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StackTrace",
                table: "ExcepcionLogs",
                newName: "Json");
        }
    }
}
