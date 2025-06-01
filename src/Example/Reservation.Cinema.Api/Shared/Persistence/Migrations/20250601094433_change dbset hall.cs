using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Cinema.Api.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changedbsethall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_CinemaHall_CinemaHallId",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CinemaHall",
                table: "CinemaHall");

            migrationBuilder.RenameTable(
                name: "CinemaHall",
                newName: "CinemaHalls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CinemaHalls",
                table: "CinemaHalls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_CinemaHalls_CinemaHallId",
                table: "Seats",
                column: "CinemaHallId",
                principalTable: "CinemaHalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_CinemaHalls_CinemaHallId",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CinemaHalls",
                table: "CinemaHalls");

            migrationBuilder.RenameTable(
                name: "CinemaHalls",
                newName: "CinemaHall");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CinemaHall",
                table: "CinemaHall",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_CinemaHall_CinemaHallId",
                table: "Seats",
                column: "CinemaHallId",
                principalTable: "CinemaHall",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
