using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class OrderInstructionConfiguration : IEntityTypeConfiguration<OrderInstruction>
    {
        public void Configure(EntityTypeBuilder<OrderInstruction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DayOfMonth).IsRequired();
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.OrderInstructions)
                   .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.NotificationChannels)
                   .WithOne(x => x.OrderInstruction)
                   .HasForeignKey(x => x.OrderInstructionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(nameof(OrderInstruction));
        }
    }
}
