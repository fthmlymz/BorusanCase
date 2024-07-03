using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.OrderInstructions.Commands.CreateOrderInstruction
{
    public class OrderInstructionCreatedEvent : BaseEvent, INotification
    {
        public OrderInstruction OrderInstruction { get; set; }

        public OrderInstructionCreatedEvent(OrderInstruction orderInstruction)
        {
            OrderInstruction = orderInstruction;
        }
    }
}
