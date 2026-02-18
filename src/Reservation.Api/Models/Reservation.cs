namespace Reservation.Api.Models;

public class Reservation
{
    public int Id { get; set; }
    public int ShowtimeId { get; set; }
    public List<int> SeatIds { get; set; } = new();
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
