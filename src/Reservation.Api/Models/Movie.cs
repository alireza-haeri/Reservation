namespace Reservation.Api.Models;

public class Movie
{
    public const string  TableName = "Movie";
    
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    /// <summary>Duration in minutes</summary>
    public int DurationMinutes { get; private set; }
    public string? Description { get; private set; } =  string.Empty;

    public static Movie Create(string title, int durationMinutes, string? description) => new()
    {
        Title = title,
        DurationMinutes = durationMinutes,
        Description = description
    };

    public void Update(string title, int durationMinutes, string? description)
    {
        Title = title;
        DurationMinutes = durationMinutes;
        Description = description;
    }
}
