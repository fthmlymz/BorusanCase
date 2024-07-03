namespace Application.Features.OrderInstructions.Queries.OrderInstructionInActiveSearch
{
    public record GetOrderInstructionInactivePaginationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
