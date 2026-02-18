namespace Reservation.Api.Models;

public class Movie
{
    public const string  TableName = "Movie";
    
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    /// <summary>Duration in minutes</summary>
    public int DurationMinutes { get; private set; }
    public string? Description { get; private set; } =  string.Empty;
}
