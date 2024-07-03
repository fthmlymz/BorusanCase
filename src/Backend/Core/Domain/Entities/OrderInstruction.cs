using Domain.Common;
using Shared.Interfaces;

namespace Domain.Entities
{
    public class OrderInstruction : BaseAuditableEntity, IPaginatedSearchEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int DayOfMonth { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public ICollection<NotificationChannel> NotificationChannels { get; set; }
    }
}
