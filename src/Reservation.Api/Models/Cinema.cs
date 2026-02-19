namespace Reservation.Api.Models;

public class Cinema
{
    public const string TableName = "Cinema";

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; } = string.Empty;

    // Each cinema can have multiple screens (halls)
    public List<Screen> Screens { get; private set; } = new();

    public static Cinema Create(string name, string? address) => new()
    {
        Name = name,
        Address = address
    };

    public void Update(string name, string? address)
    {
        Name = name;
        Address = address;
    }
}

public class Screen
{
    public int Id { get; private set; }
    public int CinemaId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    // Seats that belong to this screen
    public List<Seat> Seats { get; private set; } = new();

    public static Screen Create(int cinemaId, string name) => new()
    {
        CinemaId = cinemaId,
        Name = name
    };

    public void Update(string name)
    {
        Name = name;
    }

    public int SeatCount => Seats?.Count ?? 0;
}

public class Seat
{
    public int Id { get; private set; }
    public int ScreenId { get; private set; }
    public string Row { get; private set; } = string.Empty; // e.g. "A"
    public int Number { get; private set; } // seat number within row
    public bool IsAccessible { get; private set; }

    public static Seat Create(int screenId, string row, int number, bool isAccessible = false) => new()
    {
        ScreenId = screenId,
        Row = row,
        Number = number,
        IsAccessible = isAccessible
    };

    public void Update(string row, int number, bool isAccessible)
    {
        Row = row;
        Number = number;
        IsAccessible = isAccessible;
    }
}
