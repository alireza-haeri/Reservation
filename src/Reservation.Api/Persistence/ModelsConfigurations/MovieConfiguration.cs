using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Api.Models;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable(Movie.TableName);
        builder.HasKey(c=>c.Id);
        builder.Property(c=>c.Title).IsRequired().HasMaxLength(20);
        builder.Property(c=>c.Description).IsRequired(false).HasMaxLength(500);
        builder.Property(c => c.DurationMinutes).IsRequired();
    }
}