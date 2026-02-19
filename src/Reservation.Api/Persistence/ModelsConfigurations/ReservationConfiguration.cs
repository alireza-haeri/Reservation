using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Models.Reservation>
{
    public void Configure(EntityTypeBuilder<Models.Reservation> builder)
    {
        builder.ToTable(Models.Reservation.TableName);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CustomerName).IsRequired().HasMaxLength(100);

        // FK to ShowTime
        builder.HasOne(c => c.ShowTime)
               .WithMany()
               .HasForeignKey(c => c.ShowTimeId);

        // Reservation -> ReservationSeat (normalized seat mapping)
        builder.HasMany(r => r.ReservationSeats)
               .WithOne(rs => rs.Reservation)
               .HasForeignKey(rs => rs.ReservationId);
    }
}