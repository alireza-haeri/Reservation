using Microsoft.EntityFrameworkCore;
using Reservation.Api.Shared.Extensions;
using Reservation.Api.Shared.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddDbContext();

var app = builder.Build();

app.MapGet("/", async (ReservationDbContext context) =>
{
    var counter = await context.Counters.FirstOrDefaultAsync();
    counter!.Increment();
    await context.SaveChangesAsync();
    
    return Results.Json(counter);
});

app.Run();
