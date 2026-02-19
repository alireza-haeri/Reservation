using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models;
using Reservation.Api.Persistence;

namespace Reservation.Api.Endpoints;

public static class ScreenEndpoints
{
    public static IEndpointRouteBuilder MapScreenEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("Screen").WithTags("Screen");

        group.MapGet("", GetAllScreens);
        group.MapGet("/{id}", GetScreenById);
        group.MapPost("", CreateScreen);
        group.MapPut("/{id}", UpdateScreen);
        group.MapDelete("/{id}", DeleteScreen);

        return endpoints;
    }

    private static async Task<IResult> GetAllScreens(ReservationDbContext db)
    {
        var screens = await db.Screens
            .Select(s => new ScreenResponse(s.Id, s.CinemaId, s.Name, s.SeatCount))
            .ToListAsync();
        return Results.Ok(screens);
    }

    private static async Task<IResult> GetScreenById(int id, ReservationDbContext db)
    {
        var screen = await db.Screens.FindAsync(id);
        return screen is not null
            ? Results.Ok(new ScreenResponse(screen.Id, screen.CinemaId, screen.Name, screen.SeatCount))
            : Results.NotFound();
    }

    private static async Task<IResult> CreateScreen(ScreenRequest request, ReservationDbContext db)
    {
        var screen = Screen.Create(request.CinemaId, request.Name);
        db.Screens.Add(screen);
        await db.SaveChangesAsync();
        return Results.Created($"/Screen/{screen.Id}", new ScreenResponse(screen.Id, screen.CinemaId, screen.Name, screen.SeatCount));
    }

    private static async Task<IResult> UpdateScreen(int id, ScreenRequest request, ReservationDbContext db)
    {
        var screen = await db.Screens.FindAsync(id);
        if (screen is null) return Results.NotFound();

        screen.Update(request.Name);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteScreen(int id, ReservationDbContext db)
    {
        var screen = await db.Screens.FindAsync(id);
        if (screen is null) return Results.NotFound();

        db.Screens.Remove(screen);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    public record ScreenRequest(int CinemaId, string Name);
    public record ScreenResponse(int Id, int CinemaId, string Name, int SeatCount);
}
