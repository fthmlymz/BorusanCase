using Application.Features.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.OrderInstructions.Commands.CreateOrderInstruction
{
    public class OrderInstructionCreatedEventHandler : INotificationHandler<OrderInstructionCreatedEvent>
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<OrderInstructionCreatedEventHandler> _logger;

        public OrderInstructionCreatedEventHandler(NotificationService notificationService, ILogger<OrderInstructionCreatedEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(OrderInstructionCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _notificationService.SendNotification(notification.OrderInstruction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending notifications.");
            }
        }
    }
}
