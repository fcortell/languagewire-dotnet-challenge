using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Users.Entities;

namespace UserService.Infrastructure.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).HasColumnName("Id");
            builder.Property(u => u.Id)
                    .IsRequired();
            builder.HasIndex(u => u.Id);

            builder.Property(u => u.Name).HasColumnName("Name");
            builder.Property(u => u.Name)
                    .IsRequired();
            builder.HasIndex(u => u.Name);

            builder.Property(u => u.TranslationBalance).HasColumnName("TranslationBalance");
            builder.Property(u => u.TotalSpentAmount).HasColumnName("TotalSpentAmount");

            builder.Property(u => u.Email).HasColumnName("Email");
            builder.Property(u => u.Email)
                    .IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}