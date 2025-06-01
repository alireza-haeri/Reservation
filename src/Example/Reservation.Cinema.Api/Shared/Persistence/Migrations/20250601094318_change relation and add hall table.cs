using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Cinema.Api.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changerelationandaddhalltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaHallId",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CinemaHall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHall", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CinemaHallId",
                table: "Seats",
                column: "CinemaHallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_CinemaHall_CinemaHallId",
                table: "Seats",
                column: "CinemaHallId",
                principalTable: "CinemaHall",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_CinemaHall_CinemaHallId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "CinemaHall");

            migrationBuilder.DropIndex(
                name: "IX_Seats_CinemaHallId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CinemaHallId",
                table: "Seats");
        }
    }
}
