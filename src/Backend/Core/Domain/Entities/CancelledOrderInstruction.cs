using Domain.Common;
using Shared.Interfaces;

namespace Domain.Entities
{
    public class CancelledOrderInstruction : BaseAuditableEntity, IPaginatedSearchEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int OrderInstructionId { get; set; }
        public int UserId { get; set; }
        public int DayOfMonth { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public List<NotificationChannel> NotificationChannels { get; set; }
        public DateTime CancelledDate { get; set; }
    }
}
