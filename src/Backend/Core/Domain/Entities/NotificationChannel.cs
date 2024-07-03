using Domain.Common;
using Shared.Interfaces;

namespace Domain.Entities
{
    public class NotificationChannel : BaseAuditableEntity, IPaginatedSearchEntity
    {
        public string? Name { get; set; }
        public string Type { get; set; }
        public int OrderInstructionId { get; set; }
        public OrderInstruction OrderInstruction { get; set; }
    }
}
