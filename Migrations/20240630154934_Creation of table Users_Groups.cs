using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dw23787.Migrations
{
    /// <inheritdoc />
    public partial class CreationoftableUsers_Groups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupAdmins",
                columns: table => new
                {
                    UserFK = table.Column<int>(type: "int", nullable: false),
                    GroupFK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAdmins", x => new { x.UserFK, x.GroupFK });
                    table.ForeignKey(
                        name: "FK_GroupAdmins_Groups_GroupFK",
                        column: x => x.GroupFK,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAdmins_UsersApp_UserFK",
                        column: x => x.UserFK,
                        principalTable: "UsersApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupAdmins_GroupFK",
                table: "GroupAdmins",
                column: "GroupFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupAdmins");
        }
    }
}
