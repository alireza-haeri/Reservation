using Microsoft.EntityFrameworkCore;
using Reservation.Api.Shared.Extensions;
using Reservation.Api.Shared.Persistence;

object locker = new ();

var builder = WebApplication.CreateBuilder(args);

builder.AddDbContext();

var app = builder.Build();

app.MapGet("/", async (ReservationDbContext context) =>
{
    lock (locker)
    {
        var counter = context.Counters.FirstOrDefault();
        counter!.Increment();
        context.SaveChanges();
        return Results.Json(counter);
    }
});

app.Run();