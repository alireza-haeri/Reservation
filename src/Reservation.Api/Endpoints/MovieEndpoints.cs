using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models;
using Reservation.Api.Persistence;

namespace Reservation.Api.Endpoints;

public static class MovieEndpoints
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var movieGroup = endpoints.MapGroup(Movie.TableName).WithTags(Movie.TableName);

        movieGroup.MapGet("", GetAllMovies);
        movieGroup.MapGet("/{id}", GetMovieById);
        movieGroup.MapPost("", CreateMovie);
        movieGroup.MapPut("/{id}", UpdateMovie);
        movieGroup.MapDelete("/{id}", DeleteMovie);

        return endpoints;
    }

    private static async Task<IResult> GetAllMovies(ReservationDbContext db)
    {
        var movies = await db.Movies
            .Select(m => new MovieResponse(m.Id, m.Title, m.DurationMinutes, m.Description))
            .ToListAsync();
        return Results.Ok(movies);
    }

    private static async Task<IResult> GetMovieById(int id, ReservationDbContext db)
    {
        var movie = await db.Movies.FindAsync(id);
        return movie is not null
            ? Results.Ok(new MovieResponse(movie.Id, movie.Title, movie.DurationMinutes, movie.Description))
            : Results.NotFound();
    }

    private static async Task<IResult> CreateMovie(MovieRequest request, ReservationDbContext db)
    {
        var movie = Movie.Create(request.Title, request.DurationMinutes, request.Description);
        db.Movies.Add(movie);
        await db.SaveChangesAsync();
        return Results.Created($"/{Movie.TableName}/{movie.Id}", new MovieResponse(movie.Id, movie.Title, movie.DurationMinutes, movie.Description));
    }

    private static async Task<IResult> UpdateMovie(int id, MovieRequest request, ReservationDbContext db)
    {
        var movie = await db.Movies.FindAsync(id);
        if (movie is null) return Results.NotFound();

        movie.Update(request.Title, request.DurationMinutes, request.Description);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteMovie(int id, ReservationDbContext db)
    {
        var movie = await db.Movies.FindAsync(id);
        if (movie is null) return Results.NotFound();

        db.Movies.Remove(movie);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    public record MovieRequest(string Title, int DurationMinutes, string? Description);
    public record MovieResponse(int Id, string Title, int DurationMinutes, string? Description);
}

