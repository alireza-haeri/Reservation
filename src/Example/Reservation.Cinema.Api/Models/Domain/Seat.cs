namespace Reservation.Cinema.Api.Models.Domain;

public class Seat
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public int  HallId { get; set; }
    public CinemaHall Hall { get; set; }

    public ICollection<SeatReservation> SeatReservations { get; set; }

    public static Seat Create(string name, int hallId) =>
        new()
        {
            Name = name,
            HallId = hallId
        };
}