using Domain.Common;
using Shared.Interfaces;

namespace Domain.Entities
{
    public class User : BaseAuditableEntity, IPaginatedSearchEntity
    {
        public string Name { get; set; }
        public ICollection<OrderInstruction> OrderInstructions { get; set; }
    }
}
