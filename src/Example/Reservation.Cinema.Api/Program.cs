using Reservation.Cinema.Api.Endpoints;
using Reservation.Cinema.Api.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddDbContext();

var app = builder.Build();

app.MapCinemaHallEndpoints();

app.Run();