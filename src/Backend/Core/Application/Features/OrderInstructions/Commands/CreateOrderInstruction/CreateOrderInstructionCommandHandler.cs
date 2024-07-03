using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.OrderInstructions.Commands.CreateOrderInstruction
{
    public sealed class CreateOrderInstructionCommandHandler : IRequestHandler<CreateOrderInstructionCommand, Result<CreatedOrderInstructionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateOrderInstructionCommandHandler> _logger;
        private readonly IMediator _mediator;

        public CreateOrderInstructionCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<CreateOrderInstructionCommandHandler> logger,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Result<CreatedOrderInstructionDto>> Handle(CreateOrderInstructionCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError("User Not Found");
                throw new BadRequestExceptionCustom("Kullanıcı bulunamadı !");
            }

            var hasActiveOrder = await _unitOfWork.Repository<OrderInstruction>().AnyAsync(o => o.UserId == request.UserId && o.IsActive);

            if (hasActiveOrder)
            {
                _logger.LogError("User Already Has Active Order");
                throw new BadRequestExceptionCustom("Kullanıcının zaten aktif bir siparişi var !");
            }

            if (request.Amount < 500 || request.Amount > 99999)
            {
                _logger.LogError("InvalidAmount");
                throw new BadRequestExceptionCustom("500-99999 aralığında olmalıdır.");
            }

            if (request.DayOfMonth < 1 || request.DayOfMonth > 28)
            {
                _logger.LogError("InvalidDayOfMonth");
                throw new BadRequestExceptionCustom("1-28 tarih aralığında olmalıdır.");
            }

            var instruction = new OrderInstruction
            {
                Name = request.UserId.ToString(),
                UserId = request.UserId,
                DayOfMonth = request.DayOfMonth,
                Amount = request.Amount,
                IsActive = true,
                NotificationChannels = request.NotificationChannels.Select(c => new NotificationChannel { Type = c, Name = c }).ToList()
            };

            await _unitOfWork.Repository<OrderInstruction>().AddAsync(instruction);

            instruction.AddDomainEvent(new OrderInstructionCreatedEvent(instruction));
            var saveChangesTask = _unitOfWork.SaveChangesAsync(cancellationToken);

            #region Logging
            await (saveChangesTask.IsCanceled || saveChangesTask.IsFaulted ? Task.CompletedTask : saveChangesTask);

            if (saveChangesTask.IsCanceled)
            {
                _logger.LogInformation("Create Order Instruction Operation Cancelled");
                throw new BadRequestExceptionCustom("Create Order Instruction Could Not Add Record");
            }
            else if (saveChangesTask.IsCompleted)
            {
                _logger.LogInformation("Create Order Instruction Created");
            }
            else if (saveChangesTask.IsFaulted)
            {
                _logger.LogError($"{"Create Order Instruction Could Not Add Record"} {saveChangesTask.Exception}");
            }
            #endregion

            var createdOrderInstructionDto = new CreatedOrderInstructionDto
            {
                Id = instruction.Id,
                UserId = instruction.UserId,
                DayOfMonth = instruction.DayOfMonth,
                Amount = instruction.Amount,
                NotificationChannels = instruction.NotificationChannels.Select(c => new NotificationChannelDto { Type = c.Type, Name = c.Name }).ToList()
            };

            await _mediator.Publish(new OrderInstructionCreatedEvent(instruction), cancellationToken);

            _logger.LogInformation("Order Instruction Created", createdOrderInstructionDto.Id);

            return await Result<CreatedOrderInstructionDto>.SuccessAsync(createdOrderInstructionDto);
        }
    }
}