namespace Reservation.Cinema.Api.Models.Domain;

public class SeatReservation
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    
    //seat
    public int SeatId { get; set; }
    public Seat Seat { get; set; } = null!;
}