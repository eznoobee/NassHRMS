using AutoMapper;
using HRMS.Application.Common;
using HRMS.Application.DTOs.Department;
using HRMS.Application.Interfaces.Persistence;
using HRMS.Application.Interfaces.Security;
using HRMS.Application.Interfaces.Services;
using HRMS.Domain.Entities;

namespace HRMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DepartmentService(   IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository, 
                                    ICurrentUserService currentUserService, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var existing = await _departmentRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                return Result<DepartmentDto>.Fail("Department with this name already exists.");

            Employee? manager = null;
            if (dto.ManagerId.HasValue)
            {
                manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
                if (manager is null)
                    return Result<DepartmentDto>.Fail($"Manager with ID '{dto.ManagerId}' was not found.");
            }

            var department = new Department(createdby: _currentUserService.GetUserName(),
                                            name: dto.Name,
                                            description: dto.Description,
                                            manager: manager);

            manager?.UpdateDepartmentId(department.Id, "Assigned as department manager during department creation.");

            await _departmentRepository.AddAsync(department);

            var mapped = _mapper.Map<DepartmentDto>(department);
            return Result<DepartmentDto>.Ok(mapped, "Department created successfully.");
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            return Result<IEnumerable<DepartmentDto>>.Ok(mapped);
        }

        public async Task<Result<DepartmentDto>> GetDepartmentByNameAsync(string name)
        {
            var department = await _departmentRepository.GetByNameAsync(name);
            if (department == null)
                return Result<DepartmentDto>.Fail("Department not found.");

            var mapped = _mapper.Map<DepartmentDto>(department);
            return Result<DepartmentDto>.Ok(mapped);
        }
    }
}
