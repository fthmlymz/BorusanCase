using MediatR;

namespace Application.Features.OrderInstructions.Commands.CancelledOrderInstructions
{
    public class CancelOrderInstructionCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int OrderInstructionId { get; set; }
    }
}
