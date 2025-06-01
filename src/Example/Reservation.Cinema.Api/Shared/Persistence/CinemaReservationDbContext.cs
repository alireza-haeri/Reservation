using Microsoft.EntityFrameworkCore;
using Reservation.Cinema.Api.Models.Domain;

namespace Reservation.Cinema.Api.Shared.Persistence;

public class CinemaReservationDbContext(DbContextOptions<CinemaReservationDbContext> options) : DbContext(options)
{
    public DbSet<Seat> Seats { get; set; }
    public DbSet<SeatReservation> SeatReservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Seat>(seatBuilder =>
        {
            seatBuilder.HasKey(seat => seat.Id);
            seatBuilder.Property(seat => seat.Name).HasMaxLength(50).IsUnicode().IsRequired();

            seatBuilder.HasMany(s => s.SeatReservations).WithOne(sr => sr.Seat).HasForeignKey(sr => sr.SeatId);
        });

        modelBuilder.Entity<SeatReservation>(seatReservation =>
        {
            seatReservation.HasKey(sr => sr.Id);
            seatReservation.HasKey(sr => new { sr.SeatId, sr.From, sr.To });
            
            seatReservation.Property(sr=>sr.From).IsRequired();
            seatReservation.Property(sr=>sr.To).IsRequired();
        });
    }
}