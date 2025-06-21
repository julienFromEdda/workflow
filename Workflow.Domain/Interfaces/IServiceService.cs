using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;
public interface IServiceService
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service> GetByIdAsync(int id);
    Task<Service> CreateAsync(Service service);
    Task<Service> UpdateAsync(Service service);
    Task<bool> DeleteAsync(int id);
}
