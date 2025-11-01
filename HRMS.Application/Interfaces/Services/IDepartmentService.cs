using HRMS.Application.Common;
using HRMS.Application.DTOs.Department;

namespace HRMS.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<Result<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentDto dto);
        Task<Result<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync();
        Task<Result<DepartmentDto>> GetDepartmentByNameAsync(string name);
    }
}
