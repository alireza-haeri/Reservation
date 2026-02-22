using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Reservation.Api.Endpoints;

public static class ReservationEndpoints
{
    public static IEndpointRouteBuilder MapReservationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var reservationGroup = endpoints.MapGroup(Models.Reservation.TableName).WithTags(Models.Reservation.TableName);
        
        reservationGroup.MapPost("/", CreateReservation);
        reservationGroup.MapDelete("/", CancelReservation);
        
        return endpoints;
    }

    private static async Task<IResult> CreateReservation(CreateReservationRequest request, IDatabase redis)
    {
        if (request.SeatIds.Count == 0)
            return Results.BadRequest("Invalid reservation request.");

        var reservedKeys = new List<RedisKey>();
        var ttl = TimeSpan.FromMinutes(1);

        foreach (var seatId in request.SeatIds)
        {
            var key = (RedisKey)$"reservation:{request.ShowTimeId}:{seatId}";
            var set = await redis.StringSetAsync(key, request.UserId.ToString(), ttl,When.NotExists);
            if (!set)
            {
                if (reservedKeys.Count > 0)
                    await redis.KeyDeleteAsync(reservedKeys.ToArray());

                return Results.Conflict(new
                    { Message = "One or more seats are already reserved.", ConflictingSeatId = seatId });
            }

            reservedKeys.Add(key);
        }

        return Results.Created($"/{Models.Reservation.TableName}/{request.ShowTimeId}",
            new { Message = "Seats reserved", Seats = request.SeatIds, ExpiresInMinutes = 10 });
    }

    private static async Task<IResult> CancelReservation([FromBody]CancelReservationRequest request, [FromServices] IDatabase redis)
    {
        var keys = request.SeatIds.Select(seatId => (RedisKey)$"reservation:{request.ShowTimeId}:{seatId}").ToList();
        await redis.KeyDeleteAsync(keys.ToArray());
        
        return Results.NoContent();
    }

    public record CreateReservationRequest(int UserId, List<int> SeatIds, int ShowTimeId);
    public record CancelReservationRequest(int UserId, List<int> SeatIds, int ShowTimeId);
}