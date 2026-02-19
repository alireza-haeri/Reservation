using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Api.Models;

namespace Reservation.Api.Persistence.ModelsConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(User.TableName);
        builder.HasKey(x => x.Id);

        builder.HasMany(u => u.Reservations).WithOne(u => u.User).HasForeignKey(u => u.UserId);
    }
}