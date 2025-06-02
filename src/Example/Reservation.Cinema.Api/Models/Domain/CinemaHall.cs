namespace Reservation.Cinema.Api.Models.Domain;

public class CinemaHall
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<Seat> Seats { get; set; } = null!;

    public static CinemaHall Create(string name) =>
        new()
        {
            Name = name
        };
}