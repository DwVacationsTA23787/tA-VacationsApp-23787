using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dw23787.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserModelpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "UsersApp",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UsersApp",
                newName: "password");
        }
    }
}
