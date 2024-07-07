using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dw23787.Migrations
{
    /// <inheritdoc />
    public partial class updateAtributesTripsUsersMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nacionalidade",
                table: "UsersApp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nacionalidade",
                table: "UsersApp");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Messages");
        }
    }
}
