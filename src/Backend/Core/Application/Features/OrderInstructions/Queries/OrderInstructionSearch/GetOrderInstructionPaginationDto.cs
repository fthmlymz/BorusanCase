namespace Application.Features.OrderInstructions.Queries.OrderInstructionSearch
{
    public record GetOrderInstructionPaginationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
