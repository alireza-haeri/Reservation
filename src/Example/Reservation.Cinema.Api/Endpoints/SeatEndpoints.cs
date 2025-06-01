using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;
using Reservation.Cinema.Api.Shared.Persistence;

namespace Reservation.Cinema.Api.Endpoints
{
    public static class SeatEndpoints
    {
        public static void MapSeatEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/seats", async (CinemaReservationDbContext db) =>
                await db.Seats.Include(s => s.SeatReservations).ToListAsync()
            );

            routes.MapGet("/seats/{id}", async (int id, CinemaReservationDbContext db) =>
                await db.Seats.Include(s => s.SeatReservations).FirstOrDefaultAsync(s => s.Id == id)
                    is Seat seat ? Results.Ok(seat) : Results.NotFound()
            );

            routes.MapPost("/seats", async (Seat seat, CinemaReservationDbContext db) =>
            {
                db.Seats.Add(seat);
                await db.SaveChangesAsync();
                return Results.Created($"/seats/{seat.Id}", seat);
            });

            routes.MapPut("/seats/{id}", async (int id, Seat input, CinemaReservationDbContext db) =>
            {
                var seat = await db.Seats.FindAsync(id);
                if (seat is null) return Results.NotFound();
                seat.Name = input.Name;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            routes.MapDelete("/seats/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var seat = await db.Seats.FindAsync(id);
                if (seat is null) return Results.NotFound();
                db.Seats.Remove(seat);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}