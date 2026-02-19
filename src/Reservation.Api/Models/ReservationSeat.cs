namespace Reservation.Api.Models;

public class ReservationSeat
{
    public int Id { get; private set; }
    public int ReservationId { get; private set; }
    public int SeatId { get; private set; }

    public Reservation Reservation { get; private set; } = new();
    public Seat Seat { get; private set; } = new();

    public static ReservationSeat Create(int reservationId, int seatId) => new()
    {
        ReservationId = reservationId,
        SeatId = seatId
    };
}