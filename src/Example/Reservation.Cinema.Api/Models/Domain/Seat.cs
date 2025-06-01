namespace Reservation.Cinema.Api.Models.Domain;

public class Seat
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<SeatReservation> SeatReservations { get; set; }
}