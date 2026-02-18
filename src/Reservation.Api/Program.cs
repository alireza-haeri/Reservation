using Microsoft.EntityFrameworkCore;
using Reservation.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ReservationSettings>(builder.Configuration);
var settings = builder.Services.BuildServiceProvider().GetRequiredService<ReservationSettings>() ??
               throw new ArgumentNullException(nameof(ReservationSettings));

builder.Services.AddDbContext<ReservationDbContext>(options =>
{
    options.UseSqlite(settings.Database.ConnectionString);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();