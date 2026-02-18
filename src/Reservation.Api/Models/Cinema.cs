namespace Reservation.Api.Models;

public class Cinema
{
    public const string TableName = "Cinema";
    
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; } = string.Empty;
    public int ShowTimeId { get; private set; }
}
