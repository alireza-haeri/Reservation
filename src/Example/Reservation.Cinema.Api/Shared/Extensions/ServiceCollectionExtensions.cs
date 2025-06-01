using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Shared.Persistence;

namespace Reservation.Cinema.Api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CinemaReservationDbContext>(options =>
            options.UseSqlServer(builder.Configuration["ConnectionStrings:Sql"]));

        return builder;
    }
}