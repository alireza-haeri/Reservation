using Reservation.Api.Models.Domain;

namespace Reservation.Api.Shared.Persistence;

public static class ReservationDbContextSeedData
{
    public static List<Counter> Counters { get; set; } = 
    [
        Counter.Create()
    ];
}