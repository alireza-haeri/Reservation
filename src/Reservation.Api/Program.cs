using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reservation.Api.Persistence;
using Reservation.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ReservationSettings>(builder.Configuration);
var settings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ReservationSettings>>().Value ??
               throw new ArgumentNullException(nameof(ReservationSettings));

builder.Services.AddDbContext<ReservationDbContext>(options =>
{
    options.UseSqlite(settings.Database.ConnectionString);
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

// Map domain endpoints
app
    .MapCinemaEndpoints() 
    .MapScreenEndpoints()
    .MapSeatEndpoints()
    .MapUserEndpoints();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();

app.Run();