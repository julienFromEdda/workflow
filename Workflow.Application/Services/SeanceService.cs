using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services;

public class SeanceService(WFContext context) : ISeanceService
{
    public async Task<Seance> CreerSeanceAsync(Seance seance)
    {
        context.Seances.Add(seance);
        await context.SaveChangesAsync();
        return seance;
    }

    public async Task<Seance> ModifierSeanceAsync(Seance seance)
    {
        context.Seances.Update(seance);
        await context.SaveChangesAsync();
        return seance;
    }

    public async Task<bool> SupprimerSeanceAsync(int id)
    {
        var s = await context.Seances.FindAsync(id);
        if (s == null) return false;
        context.Seances.Remove(s);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Seance?> GetByIdAsync(int id)
    {
        var seances =  await context.Seances
            .Include(s => s.POJs)
                .ThenInclude(p => p.Dossier)
            .Include(s => s.POJs)
                .ThenInclude(p => p.Votes)
            .Include(s => s.POJs)
                .ThenInclude(p => p.Documents)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (seances?.POJs != null)
        {
            foreach (var p in seances.POJs)
            {
                p.Documents = await context.Documents
                    .Where(d => d.TypeObjet == "POJ" && d.ObjetId == p.Id)
                    .ToListAsync();
            }
        }


        return seances;
    }

    public async Task<IEnumerable<Seance>> GetByTypeAsync(TypeSeance type)
    {
        return await context.Seances
            .Where(s => s.Type == type)
            .Include(s => s.POJs)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }
}
