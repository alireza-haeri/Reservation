using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using Reservation.Api.Shared.Extensions;
using Reservation.Api.Shared.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddDbContext()
    .AddRedLock();

var app = builder.Build();

app.MapGet("/", async (ReservationDbContext context, RedLockFactory redLockFactory) =>
{
    const string lockResourceName = "counter:1:lock";
    var expireTime = TimeSpan.FromSeconds(20);
    var waitTime = TimeSpan.FromSeconds(10);
    var retryTime = TimeSpan.FromMicroseconds(10);

    await using var redLock = await redLockFactory.CreateLockAsync(lockResourceName, expireTime, waitTime, retryTime);
    
    if (!redLock.IsAcquired)
    {
        return Results.BadRequest();
    }
    
    var counter = await context.Counters.FirstOrDefaultAsync();
    counter!.Increment();
    await context.SaveChangesAsync();
        
    return Results.Json(counter);
});

app.Run();