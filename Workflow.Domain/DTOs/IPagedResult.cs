namespace Workflow.Domain.DTOs;

public interface IPagedResult
{
    int Page { get; }
    int PageSize { get; }
    int TotalItems { get; }
    int TotalPages { get; }
}
