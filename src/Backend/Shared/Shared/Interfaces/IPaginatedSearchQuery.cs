namespace Shared.Interfaces
{
    public interface IPaginatedSearchQuery
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}
