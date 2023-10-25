using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(30).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(30).IsRequired();
        builder.Property(p => p.Password).HasMaxLength(16).IsRequired();
    }
}
