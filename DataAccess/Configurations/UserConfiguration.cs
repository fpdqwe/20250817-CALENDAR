using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("UserId").ValueGeneratedNever();
            builder.Property(x => x.Password).IsRequired(true);
            builder.HasIndex(x => x.Login).IsUnique(true);
            builder.Property(x => x.Name).HasColumnName("Firstname").HasMaxLength(200);
            builder.Property(x => x.Lastname).HasColumnName("Lastname").HasMaxLength(200);
        }
    }
}
