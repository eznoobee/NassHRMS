using AutoMapper;
using HRMS.Application.Common;
using HRMS.Application.DTOs.Auth;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces.Persistence;
using HRMS.Application.Interfaces.Security;
using HRMS.Application.Interfaces.Services;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;


namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            ICurrentUserService currentUserService,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<LoginResponseDto>> LogInAsync(LoginRequestDto dto)
        {
            var result = await VerifyCredentialsAsync(dto.Identifier, dto.Password);
            if (!result.Success)
                return Result<LoginResponseDto>.Fail(result.Message?? "Unauthorized");

            var employee = result.Data!;
            var token = _jwtService.GenerateToken(
                employee.Id,
                employee.Role.ToString(),
                employee.Email,
                employee.PhoneNumber,
                employee.Username
            );

            var response = new LoginResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(8)
            };

            return Result<LoginResponseDto>.Ok(response, "Login successful.");
        }

        public async Task<Result<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var existingByUsername = await _employeeRepository.GetByUsernameAsync(dto.Username);
            if (existingByUsername != null)
                return Result<EmployeeDto>.Fail("An employee with this username already exists.");

            var existingByPhoneNumber = await _employeeRepository.GetByPhoneAsync(dto.PhoneNumber);
            if (existingByPhoneNumber != null)
                return Result<EmployeeDto>.Fail("An employee with this phone number already exists.");

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var existingByEmail = await _employeeRepository.GetByEmailAsync(dto.Email);
                if (existingByEmail != null)
                    return Result<EmployeeDto>.Fail("An employee with this email already exists.");
            }

            var passwordHash = _passwordHasher.HashPassword(dto.PasswordHash);

            var employee = new Employee(
                createdBy:          _currentUserService.GetUserName(),
                username:           dto.Username,
                phoneNumber:        dto.PhoneNumber,
                email:              dto.Email,
                passwordHash:       passwordHash,
                role:               Enum.Parse<Role>(dto.Role, true),
                firstName:          dto.FirstName,
                fatherName:         dto.FatherName,
                grandFatherName:    dto.GrandFatherName,
                lastName:           dto.LastName,
                gender:             Enum.Parse<Gender>(dto.Gender, true),
                degree:             Enum.Parse<DegreeType>(dto.CurrentDegree, true), 
                serviceInYears:     dto.ServiceInYears,
                baseSalary:         dto.BaseSalary,
                departmentId:       dto.DepartmentId
            );

            await _employeeRepository.AddAsync(employee);

            var mapped = _mapper.Map<EmployeeDto>(employee);
            return Result<EmployeeDto>.Ok(mapped, "Employee created successfully.");
        }

        public async Task<PaginatedResult<EmployeeDto>> GetPaginatedEmployeesAsync(FilterEmployeeDto dto)
        {
            var (employees, totalCount) = await _employeeRepository.GetPaginatedEmployeesAsync(dto.PageNumber, dto.PageSize, dto.SearchKeyword);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return new PaginatedResult<EmployeeDto>(employeeDtos, totalCount, dto.PageNumber, dto.PageSize);
        }

        public async Task<Result<EmployeeDto>> GetByIdAsync()
        {
            var employeeId = _currentUserService.GetUserId();

            if (employeeId == null)
                return Result<EmployeeDto>.Fail("Unauthorized: user ID not found in token.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId.Value);
            if (employee == null)
                return Result<EmployeeDto>.Fail("Employee not found.");

            var mapped = _mapper.Map<EmployeeDto>(employee);
            return Result<EmployeeDto>.Ok(mapped);
        }

        public async Task<Result<EmployeeDto>> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return Result<EmployeeDto>.Fail("Employee not found.");

            var mapped = _mapper.Map<EmployeeDto>(employee);
            return Result<EmployeeDto>.Ok(mapped);
        }

        public async Task<Result<EmployeeDto>> UpdateEmployeeAsync(Guid employeeId, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee is null)
                return Result<EmployeeDto>.Fail("Employee not found.");

            if (!string.IsNullOrWhiteSpace(dto.Username))
                employee.UpdateUsername(dto.Username, _currentUserService.GetUserName());

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                employee.UpdatePhoneNumber(dto.PhoneNumber, _currentUserService.GetUserName());

            if (dto.Email != null)
                employee.UpdateEmail(dto.Email, _currentUserService.GetUserName());

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var newHash = _passwordHasher.HashPassword(dto.Password);
                employee.UpdatePassword(newHash, _currentUserService.GetUserName());
            }

            if (!string.IsNullOrWhiteSpace(dto.Role))
                employee.UpdateRole(Enum.Parse<Role>(dto.Role, true), _currentUserService.GetUserName());

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                employee.UpdateFirstName(dto.FirstName, _currentUserService.GetUserName());

            if (!string.IsNullOrWhiteSpace(dto.FatherName))
                employee.UpdateFatherName(dto.FatherName, _currentUserService.GetUserName());

            if (dto.GrandFatherName != null)
                employee.UpdateGrandFatherName(dto.GrandFatherName, _currentUserService.GetUserName());

            if (dto.LastName != null)
                employee.UpdateLastName(dto.LastName, _currentUserService.GetUserName());

            if (!string.IsNullOrWhiteSpace(dto.Gender))
                employee.UpdateGender(Enum.Parse<Gender>(dto.Gender, true), _currentUserService.GetUserName());

            if (!string.IsNullOrWhiteSpace(dto.CurrentDegree))
                employee.UpdateDegree(Enum.Parse<DegreeType>(dto.CurrentDegree, true), _currentUserService.GetUserName());

            if (dto.ServiceInYears.HasValue)
                employee.UpdateServiceInYears(dto.ServiceInYears.Value, _currentUserService.GetUserName());

            if (dto.BaseSalary.HasValue)
                employee.UpdateBaseSalary(dto.BaseSalary.Value, _currentUserService.GetUserName());

            if (dto.DepartmentId.HasValue)
            {
                var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId.Value);
                if (department == null)
                    return Result<EmployeeDto>.Fail("Department not found.");

                employee.UpdateDepartmentId(dto.DepartmentId.Value, _currentUserService.GetUserName());
                employee.UpdateDepartment(department, _currentUserService.GetUserName());
            }

            await _employeeRepository.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            EmployeeDto updatedDto = _mapper.Map<EmployeeDto>(employee);

            return Result<EmployeeDto>.Ok(updatedDto);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return Result<bool>.Fail("Employee not found.");

            await _employeeRepository.DeleteAsync(employee);
            return Result<bool>.Ok(true, "Employee deleted successfully.");
        }

        public async Task<Result<EmployeeDto>> VerifyCredentialsAsync(string identifier, string password)
        {
            var employee = await _employeeRepository.FindByIdentifierAsync(identifier);

            if (employee == null)
                return Result<EmployeeDto>.Fail("No user found with the given identifier.");

            if (!_passwordHasher.VerifyPassword(password, employee.PasswordHash))
                return Result<EmployeeDto>.Fail("Invalid password.");

            var dto = _mapper.Map<EmployeeDto>(employee);
            return Result<EmployeeDto>.Ok(dto);
        }
    }
}
