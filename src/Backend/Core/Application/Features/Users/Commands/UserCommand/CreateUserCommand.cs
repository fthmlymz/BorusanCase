using Application.Features.Users.Dto;
using MediatR;
using Shared;

namespace Application.Features.Users.Commands.UserCommand
{
    public class CreateUserCommand : IRequest<Result<CreatedUserDto>>
    {
        public string Name { get; set; }
    }
}
