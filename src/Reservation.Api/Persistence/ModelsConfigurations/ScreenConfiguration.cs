using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Api.Models;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class ScreenConfiguration : IEntityTypeConfiguration<Screen>
{
    public void Configure(EntityTypeBuilder<Screen> builder)
    {
        builder.ToTable("Screen");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(50);

        builder.HasOne<Cinema>().WithMany(c => c.Screens).HasForeignKey(s => s.CinemaId);
        builder.HasMany(s => s.Seats).WithOne().HasForeignKey(seat => seat.ScreenId);
    }
}
