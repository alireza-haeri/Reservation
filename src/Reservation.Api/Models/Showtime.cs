namespace Reservation.Api.Models;

public class ShowTime
{
    public const string TableName = "ShowTime";
    
    public int Id { get; private set; }
    public int MovieId { get; private set; }
    public int ScreenId { get; private set; }
    public DateTime StartTime { get; private set; }
    public decimal Price { get; private set; }

    public Movie Movie { get; private set; } 
    public Screen Screen { get; private set; } 

    public static ShowTime Create(int movieId,  int screenId, DateTime startTime, decimal price) => new()
    {
        MovieId = movieId,
        ScreenId = screenId,
        StartTime = startTime,
        Price = price
    };

    public void Update(int movieId, int screenId, DateTime startTime, decimal price)
    {
        MovieId = movieId;
        ScreenId = screenId;
        StartTime = startTime;
        Price = price;
    }
}
