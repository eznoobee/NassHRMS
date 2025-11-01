using AutoMapper.QueryableExtensions;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _context;

        public EmployeeRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetByUsernameAsync(string username)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Username.ToLower() == username.ToLower());
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        }

        public async Task<Employee?> GetByPhoneAsync(string phone)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.PhoneNumber == phone);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }
        public async Task<Employee?> FindByIdentifierAsync(string identifier)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.Email == identifier ||
                    e.PhoneNumber == identifier ||
                    e.Username == identifier);
        }
        public async Task<(IEnumerable<Employee>, int)> GetPaginatedEmployeesAsync(int pageNumber, int pageSize, string? search = null)
        {
            var query = _context.Employees.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalized = search.Trim().ToLower();
                query = query.Where(e =>
                    e.FullName.ToLower().Contains(normalized) ||
                    e.Username.ToLower().Contains(normalized) ||
                    e.PhoneNumber.ToLower().Contains(normalized) ||
                    e.Email.ToLower().Contains(normalized));
            }
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
