namespace Reservation.Api.Models;

public class Reservation
{
    public const string  TableName = "Reservation";
    
    public int Id { get; private set; }
    // Foreign key to ShowTime
    public int ShowTimeId { get; private set; }
    public int UserId { get; set; }

    // Normalized reservation-seat mapping
    public List<ReservationSeat> ReservationSeats { get; private set; } = [];
    public string CustomerName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public ShowTime ShowTime { get; private set; } = new();
    public User User { get; set; } = new();
}
