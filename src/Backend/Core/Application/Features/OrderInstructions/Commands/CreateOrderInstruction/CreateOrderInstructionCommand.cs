using MediatR;
using Shared;

namespace Application.Features.OrderInstructions.Commands.CreateOrderInstruction
{
    public sealed record CreateOrderInstructionCommand(int UserId, int DayOfMonth, decimal Amount, List<string>? NotificationChannels) : IRequest<Result<CreatedOrderInstructionDto>>;
}
