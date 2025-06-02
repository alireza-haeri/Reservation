using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;
using Reservation.Cinema.Api.Shared.Persistence;
using static Reservation.Cinema.Api.Endpoints.CinemaHallEndpoints;
using static Reservation.Cinema.Api.Endpoints.CinemaHallDtoTypes;
using Reservation.Cinema.Api.Endpoints;

namespace Reservation.Cinema.Api.Endpoints
{
    public static class SeatEndpoints
    {
        public static WebApplication MapSeatEndpoints(this WebApplication app)
        {
            var seatGroup = app.MapGroup("Seat").WithTags("Seat");
            
            seatGroup.MapGet("/seats", async (CinemaReservationDbContext db) =>
            {
                var seats = await db.Seats.Include(s => s.SeatReservations)
                    .Select(s => new SeatDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        SeatReservations = s.SeatReservations.Select(sr => new SeatReservationDto
                        {
                            Id = sr.Id,
                            From = sr.From,
                            To = sr.To
                        }).ToList()
                    })
                    .ToListAsync();
                return Results.Ok(seats);
            });

            seatGroup.MapGet("/seats/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var seat = await db.Seats.Include(s => s.SeatReservations)
                    .Select(s => new SeatDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        SeatReservations = s.SeatReservations.Select(sr => new SeatReservationDto
                        {
                            Id = sr.Id,
                            From = sr.From,
                            To = sr.To
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(s => s.Id == id);
                return seat is not null ? Results.Ok(seat) : Results.NotFound();
            });

            seatGroup.MapPost("/seats", async (CreateSeatRequest request, CinemaReservationDbContext db) =>
            {
                var seat = Seat.Create(request.Name, request.HallId);
                db.Seats.Add(seat);
                await db.SaveChangesAsync();
                return Results.Created($"/seats/{seat.Id}", request);
            });

            seatGroup.MapPut("/seats/{id}", async (int id, Seat input, CinemaReservationDbContext db) =>
            {
                var seat = await db.Seats.FindAsync(id);
                if (seat is null) return Results.NotFound();
                seat.Name = input.Name;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            seatGroup.MapDelete("/seats/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var seat = await db.Seats.FindAsync(id);
                if (seat is null) return Results.NotFound();
                db.Seats.Remove(seat);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }
    }

    public record CreateSeatRequest(string Name, int HallId);
}