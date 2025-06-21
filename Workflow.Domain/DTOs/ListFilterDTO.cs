namespace Workflow.Domain.DTOs;

public class ListFilterDTO
{
    public string? Recherche { get; set; }
    public string? TriPar { get; set; }
    public bool TriDesc { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
