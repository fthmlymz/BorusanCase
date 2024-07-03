using MediatR;
using Shared;
using Shared.Interfaces;

namespace Application.Features.OrderInstructions.Queries.OrderInstructionSearch
{
    public class GetOrderInstructionPaginationQuery : IRequest<PaginatedResult<GetOrderInstructionPaginationDto>>, IPaginatedSearchQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public GetOrderInstructionPaginationQuery() { }

        public GetOrderInstructionPaginationQuery(int pageNumber, int pageSize, int userId, string? name)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            UserId = userId;
            Name = name;
        }
    }
}
