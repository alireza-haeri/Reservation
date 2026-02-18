namespace Reservation.Api.Persistence;

public class ReservationSettings
{
    public required DatabaseSettings Database { get; set; }
}

public class DatabaseSettings
{
    public required string ConnectionString { get; set; }
}