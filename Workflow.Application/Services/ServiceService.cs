using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Exceptions;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services
{
    public class ServiceService(WFContext context) : IServiceService
    {
        public async Task<Service> CreateAsync(Service service)
        {
            context.Services.Add(service);
            await context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await GetByIdAsync(id);
            context.Services.Remove(service);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await context.Services.ToListAsync();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            var service = await context.Services
                .Include(s => s.Employes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                throw new ApiException("Service non trouvé !", 404);

            return service;
        }

        public async Task<Service> UpdateAsync(Service service)
        {
            context.Services.Update(service);
            await context.SaveChangesAsync();
            return service;
        }
    }
}
