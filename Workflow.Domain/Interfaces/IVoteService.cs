using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;

public interface IVoteService
{
    Task<Vote> AjouterVoteAsync(Vote vote);
    Task<Vote> ModifierVoteAsync(Vote vote);
    Task<bool> SupprimerVoteAsync(int id);
    Task<IEnumerable<Vote>> GetVotesByPOJAsync(int pojId);
}
