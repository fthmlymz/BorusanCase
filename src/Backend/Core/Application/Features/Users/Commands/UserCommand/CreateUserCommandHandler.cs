using Application.Common.Exceptions;
using Application.Features.Users.Dto;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Users.Commands.UserCommand
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreatedUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork, 
            ILogger<CreateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<CreatedUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User { Name = request.Name };

            await _unitOfWork.Repository<User>().AddAsync(user);

            var saveChangesTask = _unitOfWork.SaveChangesAsync(cancellationToken);

            #region Logging
            await (saveChangesTask.IsCanceled || saveChangesTask.IsFaulted ? Task.CompletedTask : saveChangesTask);

            if (saveChangesTask.IsCanceled)
            {
                _logger.LogInformation("CreateUserOperationCancelled");
                throw new BadRequestExceptionCustom($"{request.Name} {"CreateUserCouldNotAddRecord"}");
            }
            else if (saveChangesTask.IsCompleted)
            {
                _logger.LogInformation($"{user.Id} {"CreateUserUserCreated"}");
            }
            else if (saveChangesTask.IsFaulted)
            {
                _logger.LogError($"{"CreateUserCouldNotAddRecord"} {saveChangesTask.Exception}");
            }
            #endregion

            var createdUserDto = user.Adapt<CreatedUserDto>();

            _logger.LogInformation($"{createdUserDto.Id} - {createdUserDto.Name} {"CreateUserUserCreated"}");

            return await Result<CreatedUserDto>.SuccessAsync(createdUserDto, "CreateUserUserCreated");
        }
    }
}
