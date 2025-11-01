using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Persistence
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee?> GetByUsernameAsync(string username);
        Task<Employee?> GetByEmailAsync(string email);
        Task<Employee?> GetByPhoneAsync(string phone);
        Task<Employee?> FindByIdentifierAsync(string identifier);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<(IEnumerable<Employee>, int)> GetPaginatedEmployeesAsync(int pageNumber, int pageSize, string? search = null);
    }
}
