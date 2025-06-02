using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;
using Reservation.Cinema.Api.Shared.Persistence;
using static Reservation.Cinema.Api.Endpoints.CinemaHallDtoTypes;

namespace Reservation.Cinema.Api.Endpoints
{
    public static class CinemaHallEndpoints
    {
        public static WebApplication MapCinemaHallEndpoints(this WebApplication app)
        {
            var hallGroup = app.MapGroup("Hall").WithTags("CinemaHall");
            
            hallGroup.MapGet("/cinemahalls", async (CinemaReservationDbContext db) =>
                {
                    var halls = await db.CinemaHalls.Include(h => h.Seats)
                        .Select(h => new CinemaHallDto
                        {
                            Id = h.Id,
                            Name = h.Name,
                            Seats = h.Seats.Select(s => new SeatDto
                            {
                                Id = s.Id,
                                Name = s.Name,
                                SeatReservations = s.SeatReservations.Select(sr => new SeatReservationDto
                                {
                                    Id = sr.Id,
                                    From = sr.From,
                                    To = sr.To
                                }).ToList()
                            }).ToList()
                        })
                        .ToListAsync();
                    return Results.Ok(halls);
                }
            );

            hallGroup.MapGet("/cinemahalls/{id}", async (int id, CinemaReservationDbContext db) =>
                {
                    var hall = await db.CinemaHalls.Include(h => h.Seats)
                        .Select(h => new CinemaHallDto
                        {
                            Id = h.Id,
                            Name = h.Name,
                            Seats = h.Seats.Select(s => new SeatDto
                            {
                                Id = s.Id,
                                Name = s.Name,
                                SeatReservations = s.SeatReservations.Select(sr => new SeatReservationDto
                                {
                                    Id = sr.Id,
                                    From = sr.From,
                                    To = sr.To
                                }).ToList()
                            }).ToList()
                        })
                        .FirstOrDefaultAsync(h => h.Id == id);
                    return hall is not null ? Results.Ok(hall) : Results.NotFound();
                }
            );
            
            hallGroup.MapPost("/cinemahalls", async (CreateCinemaHallRequest request, CinemaReservationDbContext db) =>
            {
                var cinemaHall = CinemaHall.Create(request.Name);
                db.CinemaHalls.Add(cinemaHall);
                await db.SaveChangesAsync();
                return Results.Created($"/cinemahalls/{cinemaHall.Id}", request);
            });

            hallGroup.MapPut("/cinemahalls/{id}", async (int id, CinemaHall input, CinemaReservationDbContext db) =>
            {
                var hall = await db.CinemaHalls.FindAsync(id);
                if (hall is null) return Results.NotFound();
                hall.Name = input.Name;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            hallGroup.MapDelete("/cinemahalls/{id}", async (int id, CinemaReservationDbContext db) =>
            {
                var hall = await db.CinemaHalls.FindAsync(id);
                if (hall is null) return Results.NotFound();
                db.CinemaHalls.Remove(hall);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }
    }

    public static class CinemaHallDtoTypes
    {
        public class CinemaHallDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<SeatDto> Seats { get; set; } = new();
        }
        public class SeatDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<SeatReservationDto> SeatReservations { get; set; } = new();
        }
        public class SeatReservationDto
        {
            public int Id { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }
    }

    public record CreateCinemaHallRequest(string Name);
}