using HRMS.Domain.Entities;
using HRMS.Domain.Enums;

namespace HRMS.Application.Interfaces.Persistence
{
    public interface ILeaveRepository
    {
        Task AddAsync(Leave leave);
        Task<Leave?> GetByIdAsync(Guid id);
        Task UpdateAsync(Leave leave);
        Task<(IEnumerable<Leave> Leaves, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, LeaveStatus? status, string? search = null);
        Task<IEnumerable<Leave>> GetByEmployeeIdAsync(Guid employeeId);
    }
}
