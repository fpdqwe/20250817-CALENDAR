using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class EventParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participants").HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Event)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Color).HasMaxLength(6).IsRequired(false);
            builder.Property(x => x.WarningTimeOffset).IsRequired(true).HasDefaultValue(0);
            builder.Property(x => x.Role).IsRequired(true).HasDefaultValue("Owner");
        }
    }
}
