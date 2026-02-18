namespace Reservation.Api.Models;

public class Showtime
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int CinemaId { get; set; }
    public DateTime StartTime { get; set; }
    public decimal Price { get; set; }
}
