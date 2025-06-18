namespace Workflow.Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public string? NomFichier { get; set; }
    public string? Url { get; set; }

    public int ObjetId { get; set; }
    public string? TypeObjet { get; set; }
}
