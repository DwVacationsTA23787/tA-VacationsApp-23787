using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dw23787.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNacionalityTimeMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nacionalidade",
                table: "UsersApp",
                newName: "Nationality");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nationality",
                table: "UsersApp",
                newName: "Nacionalidade");
        }
    }
}
