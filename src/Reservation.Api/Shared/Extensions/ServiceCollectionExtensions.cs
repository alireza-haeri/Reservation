using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using Reservation.Api.Shared.Persistence;
using StackExchange.Redis;

namespace Reservation.Api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ReservationDbContext>(options =>
            options.UseSqlServer(builder.Configuration["ConnectionStrings:Sql"]));

        return builder;
    }

    public static WebApplicationBuilder AddRedLock(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:Redis"]));

        builder.Services.AddSingleton<RedLockFactory>(sp =>
            RedLockFactory.Create(
                [new RedLockMultiplexer(sp.GetRequiredService<IConnectionMultiplexer>())]));

        return builder;
    }
}