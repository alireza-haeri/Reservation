namespace Reservation.Api.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    /// <summary>Duration in minutes</summary>
    public int DurationMinutes { get; set; }
    public string? Description { get; set; }
}
