using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Api.Migrations
{
    /// <inheritdoc />
    public partial class removecinemaasShowTimeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowTime_Cinema_CinemaId",
                table: "ShowTime");

            migrationBuilder.DropIndex(
                name: "IX_ShowTime_CinemaId",
                table: "ShowTime");

            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "ShowTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "ShowTime",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShowTime_CinemaId",
                table: "ShowTime",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowTime_Cinema_CinemaId",
                table: "ShowTime",
                column: "CinemaId",
                principalTable: "Cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
