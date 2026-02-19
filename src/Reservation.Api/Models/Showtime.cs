namespace Reservation.Api.Models;

public class ShowTime
{
    public const string TableName = "ShowTime";
    
    public int Id { get; private set; }
    public int MovieId { get; private set; }
    public int CinemaId { get; private set; }
    public int ScreenId { get; private set; }
    public DateTime StartTime { get; private set; }
    public decimal Price { get; private set; }

    public Movie Movie { get; private set; } = new();
    public Cinema Cinema { get; private set; } = new();
    public Screen Screen { get; private set; } = new();
}
