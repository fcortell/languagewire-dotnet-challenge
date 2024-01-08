using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Tiers.Entities;

namespace UserService.Infrastructure.Persistence.Configuration
{
    public class TierConfiguration : IEntityTypeConfiguration<Tier>
    {
        public void Configure(EntityTypeBuilder<Tier> builder)
        {
            builder.Property(u => u.Id).HasColumnName("Id");
            builder.Property(u => u.Id)
                    .IsRequired();
            builder.HasIndex(u => u.Id);

            builder.Property(u => u.Name).HasColumnName("Name");
            builder.Property(u => u.Name)
                    .IsRequired();
            builder.HasIndex(u => u.Name);

            builder.Property(u => u.RangeStart).HasColumnName("RangeStart");
            builder.Property(u => u.RangeStart)
                    .IsRequired();

            builder.Property(u => u.RangeEnd).HasColumnName("RangeEnd");
            builder.Property(u => u.RangeEnd)
                    .IsRequired();

            builder.HasData(
    new Tier { Id = 1, Name = "Sporadic", RangeStart = 0, RangeEnd = 99 },
    new Tier { Id = 2, Name = "Advanced", RangeStart = 100, RangeEnd = 999 },
        new Tier { Id = 3, Name = "Special", RangeStart = 100, RangeEnd = int.MaxValue }

);
        }
    }
}