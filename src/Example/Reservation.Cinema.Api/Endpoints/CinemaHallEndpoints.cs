using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;
using Reservation.Cinema.Api.Shared.Persistence;

namespace Reservation.Cinema.Api.Endpoints
{
    public static class CinemaHallEndpoints
    {
        public static void MapCinemaHallEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/cinemahalls", async (CinemaReservationDbContext db) =>
                await db.CinemaHalls.Include(h => h.Seats).ToListAsync()
            );

            routes.MapGet("/cinemahalls/{id}", async (int id, CinemaReservationDbContext db) =>
                await db.CinemaHalls.Include(h => h.Seats).FirstOrDefaultAsync(h => h.Id == id)
                    is CinemaHall hall ? Results.Ok(hall) : Results.NotFound()
            );

            routes.MapPost("/cinemahalls", async (CinemaHall hall, CinemaReservationDbContext db) =>
            {
                db.CinemaHalls.Add(hall);
                await db.SaveChangesAsync();
                return Results.Created($"/cinemahalls/{hall.Id}", hall);
            });

            routes.MapPut("/cinemahalls/{id}", async (int id, CinemaHall input, CinemaReservationDbContext db) =>
            {
                var hall = await db.CinemaHalls.FindAsync(id);
                if (hall is null) return Results.NotFound();
                hall.Name = input.Name;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            routes.MapDelete("/cinemahalls/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var hall = await db.CinemaHalls.FindAsync(id);
                if (hall is null) return Results.NotFound();
                db.CinemaHalls.Remove(hall);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}