using MediatR;
using Shared;
using Shared.Interfaces;

namespace Application.Features.OrderInstructions.Queries.OrderInstructionInActiveSearch
{
    public class GetOrderInstructionInActivePaginationQuery: IRequest<PaginatedResult<GetOrderInstructionInactivePaginationDto>>, IPaginatedSearchQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string? Name { get; set; }

        public GetOrderInstructionInActivePaginationQuery() { }

        public GetOrderInstructionInActivePaginationQuery(int pageNumber, int pageSize, int userId, string? name)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            UserId = userId;
            Name = name;
        }
    }
}
