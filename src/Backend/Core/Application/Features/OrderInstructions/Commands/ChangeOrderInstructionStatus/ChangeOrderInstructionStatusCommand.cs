using MediatR;
using Shared;

namespace Application.Features.OrderInstructions.Commands.ChangeOrderInstructionStatus
{
    public class ChangeOrderInstructionStatusCommand : IRequest<Result<ChangeOrderInstructionStatusDto>>
    {
        public int UserId { get; set; }
        public int OrderInstructionId { get; set; }
        public bool IsActive { get; set; }
    }
}
