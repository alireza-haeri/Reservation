using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;
using Reservation.Cinema.Api.Shared.Persistence;
using static Reservation.Cinema.Api.Endpoints.CinemaHallDtoTypes;

namespace Reservation.Cinema.Api.Endpoints
{
    public static class SeatReservationEndpoints
    {
        public static WebApplication MapSeatReservationEndpoints(this WebApplication app)
        {
            var seatReservationGroup = app.MapGroup("SeatReservation").WithTags("SeatReservation");
            
            seatReservationGroup.MapGet("/seatreservations", async (CinemaReservationDbContext db) =>
            {
                var reservations = await db.SeatReservations.Include(r => r.Seat)
                    .Select(r => new SeatReservationDto
                    {
                        Id = r.Id,
                        From = r.From,
                        To = r.To
                    })
                    .ToListAsync();
                return Results.Ok(reservations);
            });

            seatReservationGroup.MapGet("/seatreservations/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var reservation = await db.SeatReservations.Include(r => r.Seat)
                    .Select(r => new SeatReservationDto
                    {
                        Id = r.Id,
                        From = r.From,
                        To = r.To
                    })
                    .FirstOrDefaultAsync(r => r.Id == id);
                return reservation is not null ? Results.Ok(reservation) : Results.NotFound();
            });

            seatReservationGroup.MapPost("/seatreservations", async (CreateSeatReservationRequest request, CinemaReservationDbContext db) =>
            {
                var seatReservation = SeatReservation.Create(request.SeatId, request.From, request.To);
                db.SeatReservations.Add(seatReservation);
                await db.SaveChangesAsync();
                return Results.Created($"/seatreservations/{seatReservation.Id}", request);
            });

            seatReservationGroup.MapPut("/seatreservations/{id}", async (int id, SeatReservation input, CinemaReservationDbContext db) =>
            {
                var reservation = await db.SeatReservations.FindAsync(id);
                if (reservation is null) return Results.NotFound();
                reservation.From = input.From;
                reservation.To = input.To;
                reservation.SeatId = input.SeatId;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            seatReservationGroup.MapDelete("/seatreservations/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var reservation = await db.SeatReservations.FindAsync(id);
                if (reservation is null) return Results.NotFound();
                db.SeatReservations.Remove(reservation);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }
    }

    public record CreateSeatReservationRequest(int SeatId, DateTime From, DateTime To);
}