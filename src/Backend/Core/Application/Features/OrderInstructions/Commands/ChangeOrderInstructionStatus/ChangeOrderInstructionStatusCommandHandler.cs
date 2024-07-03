using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.OrderInstructions.Commands.ChangeOrderInstructionStatus
{
    public sealed class ChangeOrderInstructionStatusCommandHandler : IRequestHandler<ChangeOrderInstructionStatusCommand, Result<ChangeOrderInstructionStatusDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChangeOrderInstructionStatusCommandHandler> _logger;

        public ChangeOrderInstructionStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<ChangeOrderInstructionStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<ChangeOrderInstructionStatusDto>> Handle(ChangeOrderInstructionStatusCommand request, CancellationToken cancellationToken)
        {
            var orderInstruction = await _unitOfWork.Repository<OrderInstruction>().GetByIdAsync(request.OrderInstructionId);

            if (orderInstruction == null || orderInstruction.UserId != request.UserId)
            {
                _logger.LogError("OrderInstruction not found or user does not have permission");
                throw new NotFoundExceptionCustom("Sipariş Talimatı bulunamadı veya kullanıcının izni yok !");
            }

            orderInstruction.IsActive = request.IsActive;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("OrderInstruction {OrderInstructionId} status changed to {Status}", request.OrderInstructionId, request.IsActive ? "Active" : "Inactive");

            var dto = new ChangeOrderInstructionStatusDto
            {
                OrderInstructionId = orderInstruction.Id,
                IsActive = orderInstruction.IsActive
            };

            return Result<ChangeOrderInstructionStatusDto>.Success(dto);
        }
    }
}
