namespace Reservation.Api.Models.Domain;

public class Counter
{
    public int Id { get; private set; }
    public int Count { get; private set; }

    public void Increment() =>
        Count++;

    public static Counter Create(int count = 0) =>
        new() { Id = 1, Count = count };
}