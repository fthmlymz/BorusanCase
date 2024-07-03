using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.OrderInstructions.Commands.CancelledOrderInstructions
{
    public sealed class CancelOrderInstructionCommandHandler : IRequestHandler<CancelOrderInstructionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelOrderInstructionCommandHandler> _logger;

        public CancelOrderInstructionCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelOrderInstructionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(CancelOrderInstructionCommand request, CancellationToken cancellationToken)
        {
            var orderInstruction = await _unitOfWork.Repository<OrderInstruction>().GetByIdAsync(request.OrderInstructionId);

            if (orderInstruction == null || orderInstruction.UserId != request.UserId)
            {
                _logger.LogError("OrderInstruction not found or user does not have permission");
                throw new BadRequestExceptionCustom("Sipariş Talimatı bulunamadı veya kullanıcının izni yok !");
            }

            var cancelledOrderInstruction = new CancelledOrderInstruction
            {
                OrderInstructionId = orderInstruction.Id,
                UserId = orderInstruction.UserId,
                DayOfMonth = orderInstruction.DayOfMonth,
                Amount = orderInstruction.Amount,
                IsActive = false,
                NotificationChannels = orderInstruction.NotificationChannels?.ToList() ?? new List<NotificationChannel>(),
                CancelledDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<CancelledOrderInstruction>().AddAsync(cancelledOrderInstruction);
            await _unitOfWork.Repository<OrderInstruction>().DeleteAsync(orderInstruction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("OrderInstruction {OrderInstructionId} has been cancelled", request.OrderInstructionId);

            return true;
        }
    }
}
