using HRMS.Application.Interfaces.Persistence;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly HRMSDbContext _context;

        public LeaveRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Leave leave)
        {
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();
        }

        public async Task<Leave?> GetByIdAsync(Guid id)
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task UpdateAsync(Leave leave)
        {
            _context.Leaves.Update(leave);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Leave> Leaves, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, LeaveStatus? status, string? search = null)
        {
            var query = _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalized = search.Trim().ToLower();
                query = query.Where(e =>
                    e.Employee.FullName.ToLower().Contains(normalized) ||
                    e.Employee.Username.ToLower().Contains(normalized) ||
                    e.Employee.Department.Name.ToLower().Contains(normalized));
            }

            var totalCount = await query.CountAsync();
            var leaves = await query
                .OrderByDescending(l => l.StartDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (leaves, totalCount);
        }
        public async Task<IEnumerable<Leave>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId)
                .ToListAsync();
        }

    }
}
