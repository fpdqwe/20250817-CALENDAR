using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(50)
                .HasDefaultValue("Event");
            builder.Property(x => x.Duration).HasDefaultValue(4).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(300)
                .IsRequired(false);
            builder.Property(x => x.Color).HasMaxLength(6).IsRequired(false);
            builder.Property(x => x.Ico).IsRequired(false);
            builder.Property(x => x.IterationTime).IsRequired(true)
                .HasDefaultValue(IterationTime.Single);
            builder.HasMany(x => x.Participants).WithOne(x => x.Event);
        }
    }
}
