using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models;
using Reservation.Api.Persistence;

namespace Reservation.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var userGroup = endpoints.MapGroup(User.TableName).WithTags(User.TableName);

        userGroup.MapPost("/register", RegisterUser);
        userGroup.MapPost("/login", LoginUser);

        return endpoints;
    }

    private static async Task<IResult> RegisterUser(RegisterRequest request, ReservationDbContext db)
    {
        // simple uniqueness check
        if (await db.Users.AnyAsync(u => u.UserName == request.UserName || u.Email == request.Email))
            return Results.Conflict(new { message = "User with same username or email already exists." });

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            NormalizedUserName = request.UserName.ToUpperInvariant(),
            NormalizedEmail = request.Email?.ToUpperInvariant()
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Results.Created($"/{User.TableName}/{user.Id}", new UserResponse(user.Id, user.UserName ?? string.Empty, user.Email));
    }

    private static async Task<IResult> LoginUser(LoginRequest request, ReservationDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.UserName);
        if (user is null) return Results.Unauthorized();

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return Results.Unauthorized();

        // Simple response — token not implemented
        return Results.Ok(new LoginResponse(user.Id, user.UserName ?? string.Empty, "login_success"));
    }

    public record RegisterRequest(string UserName, string? Email, string Password);
    public record LoginRequest(string UserName, string Password);
    public record UserResponse(int Id, string UserName, string? Email);
    public record LoginResponse(int Id, string UserName, string Token);
}
