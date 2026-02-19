using Microsoft.AspNetCore.Http.HttpResults;
using Reservation.Api.Models;
using Reservation.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Reservation.Api.Endpoints;

public static class CinemaEndpoints
{
    public static IEndpointRouteBuilder MapCinemaEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var cinemaGroup = endpoints.MapGroup(Cinema.TableName).WithTags(Cinema.TableName);

        cinemaGroup.MapGet("", GetAllCinemas);
        cinemaGroup.MapGet("/{id}", GetCinemaById);
        cinemaGroup.MapPost("", CreateCinema);
        cinemaGroup.MapPut("/{id}", UpdateCinema);
        cinemaGroup.MapDelete("/{id}", DeleteCinema);

        return endpoints;
    }

    private static async Task<IResult> GetAllCinemas(ReservationDbContext dbContext)
    {
        var cinemas = await dbContext.Cinemas
            .Select(c => new CinemaResponse(c.Id, c.Name, c.Address))
            .ToListAsync();
        return Results.Ok(cinemas);
    }

    private static async Task<IResult> GetCinemaById(int id, ReservationDbContext dbContext)
    {
        var cinema = await dbContext.Cinemas.FindAsync(id);
        return cinema is not null
            ? Results.Ok(new CinemaResponse(cinema.Id, cinema.Name, cinema.Address))
            : Results.NotFound();
    }

    private static async Task<IResult> CreateCinema(CinemaRequest request, ReservationDbContext dbContext)
    {
        var cinema = Cinema.Create(request.Name, request.Address);
        dbContext.Cinemas.Add(cinema);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/{Cinema.TableName}/{cinema.Id}", new CinemaResponse(cinema.Id, cinema.Name, cinema.Address));
    }

    private static async Task<IResult> UpdateCinema(int id, CinemaRequest request, ReservationDbContext dbContext)
    {
        var cinema = await dbContext.Cinemas.FindAsync(id);
        if (cinema is null) return Results.NotFound();

        cinema.Update(request.Name, request.Address);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteCinema(int id, ReservationDbContext dbContext)
    {
        var cinema = await dbContext.Cinemas.FindAsync(id);
        if (cinema is null) return Results.NotFound();

        dbContext.Cinemas.Remove(cinema);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }

    public record CinemaRequest(string Name, string? Address);
    public record CinemaResponse(int Id, string Name, string? Address);
}