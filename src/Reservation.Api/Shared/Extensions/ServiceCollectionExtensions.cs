using Microsoft.EntityFrameworkCore;
using Reservation.Api.Shared.Persistence;

namespace Reservation.Api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ReservationDbContext>(options=>
            options.UseSqlServer(builder.Configuration["ConnectionStrings:Sql"]));
        
        return builder;
    }
}