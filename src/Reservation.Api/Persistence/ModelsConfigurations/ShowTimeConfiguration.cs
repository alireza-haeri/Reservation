using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Api.Models;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class ShowTimeConfiguration : IEntityTypeConfiguration<ShowTime>
{
    public void Configure(EntityTypeBuilder<ShowTime> builder)
    {
        builder.ToTable(ShowTime.TableName);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(c => c.Cinema).WithMany().HasForeignKey(c => c.CinemaId);
        builder.HasOne(c => c.Movie).WithMany().HasForeignKey(c => c.MovieId);

    }
}