using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services;

public class VoteService(WFContext context) : IVoteService
{
    public async Task<Vote> AjouterVoteAsync(Vote vote)
    {
        context.Votes.Add(vote);
        await context.SaveChangesAsync();
        return vote;
    }

    public async Task<Vote> ModifierVoteAsync(Vote vote)
    {
        context.Votes.Update(vote);
        await context.SaveChangesAsync();
        return vote;
    }

    public async Task<bool> SupprimerVoteAsync(int id)
    {
        var vote = await context.Votes.FindAsync(id);
        if (vote == null) return false;
        context.Votes.Remove(vote);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Vote>> GetVotesByPOJAsync(int pojId)
    {
        return await context.Votes
            .Where(v => v.PointOrdreJourId == pojId)
            .ToListAsync();
    }
}
