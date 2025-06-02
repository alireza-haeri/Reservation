using Reservation.Cinema.Api.Endpoints;
using Reservation.Cinema.Api.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddDbContext();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app
    .MapCinemaHallEndpoints()
    .MapSeatEndpoints()
    .MapSeatReservationEndpoints();

app.Run();