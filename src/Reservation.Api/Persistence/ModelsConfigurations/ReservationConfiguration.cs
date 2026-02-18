using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Models.Reservation>
{
    public void Configure(EntityTypeBuilder<Models.Reservation> builder)
    {
        builder.ToTable(Models.Reservation.TableName);
        builder.HasKey(c=>c.Id);
        builder.Property(c=>c.CustomerName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.SeatIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v),
                v => JsonSerializer.Deserialize<List<int>>(v) ?? new ()
            )
            .HasColumnType("TEXT");

        builder.HasOne(c => c.ShowTime).WithMany().HasForeignKey(c=>c.ShowtimeId);

    }
}