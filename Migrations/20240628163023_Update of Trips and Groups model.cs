using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dw23787.Migrations
{
    /// <inheritdoc />
    public partial class UpdateofTripsandGroupsmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Trips_TripFk",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Groups_GroupFK",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_GroupFK",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TripFk",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TripFk",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "closed",
                table: "Trips",
                newName: "Closed");

            migrationBuilder.RenameColumn(
                name: "GroupFK",
                table: "Trips",
                newName: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GroupId",
                table: "Trips",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Groups_GroupId",
                table: "Trips",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Groups_GroupId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_GroupId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "Closed",
                table: "Trips",
                newName: "closed");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Trips",
                newName: "GroupFK");

            migrationBuilder.AddColumn<string>(
                name: "TripFk",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GroupFK",
                table: "Trips",
                column: "GroupFK");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TripFk",
                table: "Groups",
                column: "TripFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Trips_TripFk",
                table: "Groups",
                column: "TripFk",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Groups_GroupFK",
                table: "Trips",
                column: "GroupFK",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
