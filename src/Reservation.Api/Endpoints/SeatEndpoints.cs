using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models;
using Reservation.Api.Persistence;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Reservation.Api.Endpoints;

public static class SeatEndpoints
{
    public static IEndpointRouteBuilder MapSeatEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("Seat").WithTags("Seat");

        group.MapGet("", GetAllSeats);
        group.MapGet("/{id}", GetSeatById);
        group.MapGet("/byscreen/{screenId}", GetSeatsByScreen);
        group.MapGet("/byshowtime/{showTimeId}", GetSeatsByShowTime);
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

    private static async Task<IResult> GetSeatsByShowTime(int showTimeId, [FromServices]IDatabase redis,[FromServices] ReservationDbContext db)
    {
        var cacheKey = (RedisKey)$"showtime:{showTimeId}:seats";

        List<SeatResponse> baseSeats;

        var cached = await redis.StringGetAsync(cacheKey);
        if (cached is { HasValue: true, IsNullOrEmpty: false })
        {
            try
            {
                baseSeats = JsonSerializer.Deserialize<List<SeatResponse>>(cached.ToString()) ?? [];
            }
            catch
            {
                baseSeats = [];
            }
        }
        else
        {
            var showTime = await db.ShowTimes.FindAsync(showTimeId);
            if (showTime is null) return Results.NotFound();

            var seats = await db.Seats
                .Where(s => s.ScreenId == showTime.ScreenId)
                .ToListAsync();

            baseSeats = seats.Select(s => new SeatResponse(s.Id, s.ScreenId, s.Row, s.Number, s.IsAccessible)).ToList();

            var json = JsonSerializer.Serialize(baseSeats);
            await redis.StringSetAsync(cacheKey, json, TimeSpan.FromHours(6));
        }

        if (baseSeats.Count == 0)
            return Results.Ok(Array.Empty<object>());

        var keys = baseSeats.Select(s => (RedisKey)$"reservation:{showTimeId}:{s.Id}").ToArray();
        var values = await redis.StringGetAsync(keys);

        var result = new List<SeatWithReservedResponse>(baseSeats.Count);
        for (var i = 0; i < baseSeats.Count; i++)
        {
            var s = baseSeats[i];
            var v = values[i];
            bool reserved = v is { HasValue: true, IsNullOrEmpty: false };
            int? reservedBy = null;
            if (reserved && int.TryParse(v.ToString(), out var parsed)) reservedBy = parsed;

            result.Add(new SeatWithReservedResponse(s.Id, s.ScreenId, s.Row, s.Number, s.IsAccessible, reserved, reservedBy));
        }

        return Results.Ok(result);
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
    public record SeatWithReservedResponse(int Id, int ScreenId, string Row, int Number, bool IsAccessible, bool Reserved, int? ReservedBy);
}
