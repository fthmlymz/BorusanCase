using Application.Behaviors;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.OrderInstructions.Queries.OrderInstructionInActiveSearch
{
    internal class GetOrderInstructionInActivePaginationQueryHandler : IRequestHandler<GetOrderInstructionInActivePaginationQuery, PaginatedResult<GetOrderInstructionInactivePaginationDto>>
    {
        private readonly SearchBehavior<GetOrderInstructionInActivePaginationQuery, CancelledOrderInstruction, GetOrderInstructionInactivePaginationDto> _search;

        public GetOrderInstructionInActivePaginationQueryHandler(SearchBehavior<GetOrderInstructionInActivePaginationQuery, CancelledOrderInstruction, GetOrderInstructionInactivePaginationDto> search)
        {
            _search = search;
        }

        public async Task<PaginatedResult<GetOrderInstructionInactivePaginationDto>> Handle(GetOrderInstructionInActivePaginationQuery request, CancellationToken cancellationToken)
        {
            request.IsActive = false;
            return await _search.Handle(request, cancellationToken);
        }
    }
}
