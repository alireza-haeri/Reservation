using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models;
using Reservation.Api.Persistence;

namespace Reservation.Api.Endpoints;

public static class ShowTimeEndpoints
{
    public static IEndpointRouteBuilder MapShowTimeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var showTimeGroup = endpoints.MapGroup(ShowTime.TableName).WithTags(ShowTime.TableName);

        showTimeGroup.MapGet("", GetAllShowTimes);
        showTimeGroup.MapGet("/{id}", GetShowTimeById);
        showTimeGroup.MapPost("", CreateShowTime);
        showTimeGroup.MapPut("/{id}", UpdateShowTime);
        showTimeGroup.MapDelete("/{id}", DeleteShowTime);

        return endpoints;
    }

    private static async Task<IResult> GetAllShowTimes(ReservationDbContext db)
    {
        var list = await db.ShowTimes
            .Select(s => new ShowTimeResponse(s.Id, s.MovieId, s.Screen.CinemaId, s.ScreenId, s.StartTime, s.Price))
            .ToListAsync();
        return Results.Ok(list);
    }

    private static async Task<IResult> GetShowTimeById(int id, ReservationDbContext db)
    {
        var s = await db.ShowTimes.Include(s => s.Screen).FirstOrDefaultAsync(s => s.Id == id);
        return s is not null
            ? Results.Ok(new ShowTimeResponse(s.Id, s.MovieId, s.Screen.CinemaId, s.ScreenId, s.StartTime, s.Price))
            : Results.NotFound();
    }

    private static async Task<IResult> CreateShowTime(ShowTimeRequest request, ReservationDbContext db)
    {
        var st = ShowTime.Create(request.MovieId, request.ScreenId, request.StartTime, request.Price);
        await db.ShowTimes.AddAsync(st);
        await db.SaveChangesAsync();
        var cinemaId = await db.Screens
            .Where(s => s.Id == request.ScreenId)
            .Select(s => s.CinemaId)
            .SingleAsync();
        return Results.Created($"/{ShowTime.TableName}/{st!.Id}",
            new ShowTimeResponse(st.Id, st.MovieId, cinemaId, st.ScreenId, st.StartTime, st.Price));
    }

    private static async Task<IResult> UpdateShowTime(int id, ShowTimeRequest request, ReservationDbContext db)
    {
        var st = await db.ShowTimes.FindAsync(id);
        if (st is null) return Results.NotFound();

        st.Update(request.MovieId, request.ScreenId, request.StartTime, request.Price);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteShowTime(int id, ReservationDbContext db)
    {
        var st = await db.ShowTimes.FindAsync(id);
        if (st is null) return Results.NotFound();

        db.ShowTimes.Remove(st);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    public record ShowTimeRequest(int MovieId, int ScreenId, DateTime StartTime, decimal Price);

    public record ShowTimeResponse(int Id, int MovieId, int CinemaId, int ScreenId, DateTime StartTime, decimal Price);
}