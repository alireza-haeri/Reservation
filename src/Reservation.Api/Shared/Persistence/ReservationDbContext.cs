using Microsoft.EntityFrameworkCore;
using Reservation.Api.Models.Domain;

namespace Reservation.Api.Shared.Persistence;

public class ReservationDbContext(DbContextOptions<ReservationDbContext> options) : DbContext(options)
{
    public DbSet<Counter> Counters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Counter>()
            .HasData(ReservationDbContextSeedData.Counters);

        modelBuilder.Entity<Counter>(counterBuilder =>
        {
            counterBuilder.HasKey(c => c.Id);
        });
    }
}