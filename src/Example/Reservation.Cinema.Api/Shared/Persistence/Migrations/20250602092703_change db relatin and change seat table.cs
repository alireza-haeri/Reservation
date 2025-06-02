using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Cinema.Api.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changedbrelatinandchangeseattable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_CinemaHalls_CinemaHallId",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "CinemaHallId",
                table: "Seats",
                newName: "HallId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_CinemaHallId",
                table: "Seats",
                newName: "IX_Seats_HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_CinemaHalls_HallId",
                table: "Seats",
                column: "HallId",
                principalTable: "CinemaHalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_CinemaHalls_HallId",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "HallId",
                table: "Seats",
                newName: "CinemaHallId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_HallId",
                table: "Seats",
                newName: "IX_Seats_CinemaHallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_CinemaHalls_CinemaHallId",
                table: "Seats",
                column: "CinemaHallId",
                principalTable: "CinemaHalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
