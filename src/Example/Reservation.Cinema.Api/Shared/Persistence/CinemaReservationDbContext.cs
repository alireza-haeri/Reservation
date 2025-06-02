using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;

namespace Reservation.Cinema.Api.Shared.Persistence;

public class CinemaReservationDbContext(DbContextOptions<CinemaReservationDbContext> options) : DbContext(options)
{
    public DbSet<CinemaHall> CinemaHalls { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<SeatReservation> SeatReservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CinemaHall>(hallBuilder =>
        {
            hallBuilder.HasKey(h => h.Id);
            hallBuilder.Property(h => h.Name).HasMaxLength(100).IsRequired();
        });
        modelBuilder.Entity<Seat>(seatBuilder =>
        {
            seatBuilder.HasKey(seat => seat.Id);
            seatBuilder.Property(seat => seat.Name).HasMaxLength(50).IsUnicode().IsRequired();
        });
        modelBuilder.Entity<SeatReservation>(seatReservationBuilder =>
        {
            seatReservationBuilder.HasKey(sr => sr.Id);
            seatReservationBuilder.HasKey(sr => new { sr.SeatId, sr.From, sr.To });

            seatReservationBuilder.Property(sr => sr.From).IsRequired();
            seatReservationBuilder.Property(sr => sr.To).IsRequired();
        });
    }
}