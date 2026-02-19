using Microsoft.AspNetCore.Identity;

namespace Reservation.Api.Models;

public class User : IdentityUser<int>
{
    public const string TableName = "Users";

    public ICollection<Reservation> Reservations { get; private set; } = [];
}