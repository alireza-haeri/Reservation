using Reservation.Api.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDbContext();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
