using HRMS.Domain.Enums;

namespace HRMS.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } = default!;
        public string Role { get; set; } = default!; // enum
        public string FirstName { get; set; } = default!;
        public string FatherName { get; set; } = default!;
        public string? GrandFatherName { get; set; }
        public string? LastName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Gender { get; set; } = default!; // enum
        public string CurrentDegree { get; set; } = default!; // enum
        public int ServiceInYears { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal ServiceBouns { get; set; } // calculated
        public decimal Allowances { get; set; } // calculated
        public decimal TotalSalary { get; set; } // calculated
        public string DepartmentName { get; set; } = default!;
        public string? MaanagerName { get; set; } = default!;
    }
}
