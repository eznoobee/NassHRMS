using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Persistence
{
    public interface IDepartmentRepository
    {
        Task AddAsync(Department department);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(Guid id);
        Task<Department?> GetByNameAsync(string name);
    }
}
