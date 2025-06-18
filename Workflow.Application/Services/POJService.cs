using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services;

public class POJService(WFContext context) : IPOJService
{
    public async Task<PointOrdreJour> CreerPOJAsync(PointOrdreJour poj)
    {
        context.PointsOrdreJour.Add(poj);
        await context.SaveChangesAsync();
        return poj;
    }

    public async Task<PointOrdreJour> ModifierPOJAsync(PointOrdreJour poj)
    {
        context.PointsOrdreJour.Update(poj);
        await context.SaveChangesAsync();
        return poj;
    }

    public async Task<bool> SupprimerPOJAsync(int id)
    {
        var p = await context.PointsOrdreJour.FindAsync(id);
        if (p == null) return false;
        context.PointsOrdreJour.Remove(p);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PointOrdreJour?> GetByIdAsync(int id)
    {
        var poj =  await context.PointsOrdreJour
            .Include(p => p.Seance)
            .Include(p => p.Votes)
            .Include(p => p.Documents)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (poj == null)
            return null;

        poj.Documents = await context.Documents
            .Where(d => d.TypeObjet == "POJ" && d.ObjetId == poj.Id)
            .ToListAsync();

        return poj;
    }

    public async Task<IEnumerable<PointOrdreJour>> GetBySeanceAsync(int seanceId)
    {
        return await context.PointsOrdreJour
            .Where(p => p.SeanceId == seanceId)
            .ToListAsync();
    }
}
