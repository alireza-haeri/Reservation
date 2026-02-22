using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reservation.Api.Persistence;
using Reservation.Api.Endpoints;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ReservationSettings>(builder.Configuration);
var settings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ReservationSettings>>().Value ??
               throw new ArgumentNullException(nameof(ReservationSettings));

builder.Services.AddDbContext<ReservationDbContext>(options =>
{
    options.UseSqlite(settings.Database.ConnectionString);
});

builder.Services.AddSingleton( _ =>
{
    var conf = new ConfigurationOptions()
    {
        EndPoints = { settings.Redis.Endpoints },
        User = settings.Redis.UserName,
        Password = settings.Redis.Password
    };
    var muxer=  ConnectionMultiplexer.Connect(conf);
    return muxer.GetDatabase();
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
    .MapMovieEndpoints()
    .MapShowTimeEndpoints()
    .MapReservationEndpoints()
    .MapUserEndpoints();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();

app.Run();