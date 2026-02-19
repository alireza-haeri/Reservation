using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Api.Models;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seat");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Row).IsRequired().HasMaxLength(5);
        builder.Property(s => s.Number).IsRequired();
        builder.Property(s => s.IsAccessible).IsRequired();

        builder.HasOne<Screen>().WithMany(s => s.Seats).HasForeignKey(s => s.ScreenId);
    }
}
