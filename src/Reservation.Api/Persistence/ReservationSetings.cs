namespace Reservation.Api.Persistence;

public class ReservationSettings
{
    public required DatabaseSettings Database { get; set; }
    public required RedisSettings Redis { get; set; }
}

public class DatabaseSettings
{
    public required string ConnectionString { get; set; }
}

public class RedisSettings
{
    public required string Endpoints { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
} 