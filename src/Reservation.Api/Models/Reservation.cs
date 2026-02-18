namespace Reservation.Api.Models;

public class Reservation
{
    public const string  TableName = "Reservation";
    
    public int Id { get; private set; }
    public int ShowtimeId { get; private set; }
    public List<int> SeatIds { get; private set; } = new();
    public string CustomerName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public ShowTime ShowTime { get; private set; } = new();
}
