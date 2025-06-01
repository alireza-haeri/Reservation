using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;
using Reservation.Cinema.Api.Shared.Persistence;

namespace Reservation.Cinema.Api.Endpoints
{
    public static class SeatReservationEndpoints
    {
        public static void MapSeatReservationEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/seatreservations", async (CinemaReservationDbContext db) =>
                await db.SeatReservations.Include(r => r.Seat).ToListAsync()
            );

            routes.MapGet("/seatreservations/{id}", async (int id, CinemaReservationDbContext db) =>
                await db.SeatReservations.Include(r => r.Seat).FirstOrDefaultAsync(r => r.Id == id)
                    is { } reservation ? Results.Ok(reservation) : Results.NotFound()
            );

            routes.MapPost("/seatreservations", async (SeatReservation reservation, CinemaReservationDbContext db) =>
            {
                db.SeatReservations.Add(reservation);
                await db.SaveChangesAsync();
                return Results.Created($"/seatreservations/{reservation.Id}", reservation);
            });

            routes.MapPut("/seatreservations/{id}", async (int id, SeatReservation input, CinemaReservationDbContext db) =>
            {
                var reservation = await db.SeatReservations.FindAsync(id);
                if (reservation is null) return Results.NotFound();
                reservation.From = input.From;
                reservation.To = input.To;
                reservation.SeatId = input.SeatId;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            routes.MapDelete("/seatreservations/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var reservation = await db.SeatReservations.FindAsync(id);
                if (reservation is null) return Results.NotFound();
                db.SeatReservations.Remove(reservation);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}