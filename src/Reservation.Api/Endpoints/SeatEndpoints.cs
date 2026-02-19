using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models;
using Reservation.Api.Persistence;

namespace Reservation.Api.Endpoints;

public static class SeatEndpoints
{
    public static IEndpointRouteBuilder MapSeatEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("Seat").WithTags("Seat");

        group.MapGet("", GetAllSeats);
        group.MapGet("/{id}", GetSeatById);
        group.MapGet("/byscreen/{screenId}", GetSeatsByScreen);
        group.MapPost("", CreateSeat);
        group.MapPut("/{id}", UpdateSeat);
        group.MapDelete("/{id}", DeleteSeat);

        return endpoints;
    }

    private static async Task<IResult> GetAllSeats(ReservationDbContext db)
    {
        var seats = await db.Seats
            .Select(s => new SeatResponse(s.Id, s.ScreenId, s.Row, s.Number, s.IsAccessible))
            .ToListAsync();
        return Results.Ok(seats);
    }

    private static async Task<IResult> GetSeatById(int id, ReservationDbContext db)
    {
        var seat = await db.Seats.FindAsync(id);
        return seat is not null
            ? Results.Ok(new SeatResponse(seat.Id, seat.ScreenId, seat.Row, seat.Number, seat.IsAccessible))
            : Results.NotFound();
    }

    private static async Task<IResult> GetSeatsByScreen(int screenId, ReservationDbContext db)
    {
        var seats = await db.Seats
            .Where(s => s.ScreenId == screenId)
            .Select(s => new SeatResponse(s.Id, s.ScreenId, s.Row, s.Number, s.IsAccessible))
            .ToListAsync();
        return Results.Ok(seats);
    }

    private static async Task<IResult> CreateSeat(SeatRequest request, ReservationDbContext db)
    {
        var seat = Seat.Create(request.ScreenId, request.Row, request.Number, request.IsAccessible);
        db.Seats.Add(seat);
        await db.SaveChangesAsync();
        return Results.Created($"/Seat/{seat.Id}", new SeatResponse(seat.Id, seat.ScreenId, seat.Row, seat.Number, seat.IsAccessible));
    }

    private static async Task<IResult> UpdateSeat(int id, SeatRequest request, ReservationDbContext db)
    {
        var seat = await db.Seats.FindAsync(id);
        if (seat is null) return Results.NotFound();

        seat.Update(request.Row, request.Number, request.IsAccessible);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteSeat(int id, ReservationDbContext db)
    {
        var seat = await db.Seats.FindAsync(id);
        if (seat is null) return Results.NotFound();

        db.Seats.Remove(seat);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    public record SeatRequest(int ScreenId, string Row, int Number, bool IsAccessible);
    public record SeatResponse(int Id, int ScreenId, string Row, int Number, bool IsAccessible);
}
