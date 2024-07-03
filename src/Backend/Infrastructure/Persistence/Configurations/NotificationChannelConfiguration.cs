using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class NotificationChannelConfiguration : IEntityTypeConfiguration<NotificationChannel>
    {
        public void Configure(EntityTypeBuilder<NotificationChannel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);

            builder.HasOne(x => x.OrderInstruction)
                   .WithMany(x => x.NotificationChannels)
                   .HasForeignKey(x => x.OrderInstructionId);

            builder.ToTable(nameof(NotificationChannel));
        }
    }
}
