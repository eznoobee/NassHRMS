using HRMS.Application.Common;
using HRMS.Application.DTOs.Auth;
using HRMS.Application.DTOs.Employee;

namespace HRMS.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<Result<LoginResponseDto>> LogInAsync(LoginRequestDto dto);
        Task<Result<EmployeeDto>> GetByIdAsync();
        Task<Result<EmployeeDto>> GetByIdAsync(Guid id);
        Task<Result<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<Result<EmployeeDto>> UpdateEmployeeAsync(Guid id,UpdateEmployeeDto dto);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<PaginatedResult<EmployeeDto>> GetPaginatedEmployeesAsync(FilterEmployeeDto dto);
        Task<Result<EmployeeDto>> VerifyCredentialsAsync(string identifier, string password);
    }
}
