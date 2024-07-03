using Application.Behaviors;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.OrderInstructions.Queries.OrderInstructionSearch
{
    internal class GetOrderInstructionPaginationQueryHandler : IRequestHandler<GetOrderInstructionPaginationQuery, PaginatedResult<GetOrderInstructionPaginationDto>>
    {
        private readonly SearchBehavior<GetOrderInstructionPaginationQuery, OrderInstruction, GetOrderInstructionPaginationDto> _search;

        public GetOrderInstructionPaginationQueryHandler(SearchBehavior<GetOrderInstructionPaginationQuery, OrderInstruction, GetOrderInstructionPaginationDto> search)
        {
            _search = search;
        }

        public async Task<PaginatedResult<GetOrderInstructionPaginationDto>> Handle(GetOrderInstructionPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _search.Handle(request, cancellationToken);
        }
    }
}
