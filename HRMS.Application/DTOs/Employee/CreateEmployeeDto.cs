using HRMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        [Required] public string Username { get; set; } = default!;
        [Required, Phone] public string PhoneNumber { get; set; } = default!;
        [EmailAddress] public string Email { get; set; } = default!;
        [Required] public string PasswordHash { get; set; } = default!;
        [Required] public string Role { get; set; } = default!;
        [Required] public string FirstName { get; set; } = default!;
        [Required] public string FatherName { get; set; } = default!;
        public string? GrandFatherName { get; set; }
        public string? LastName { get; set; }
        [Required] public string Gender { get; set; } = default!;
        [Required] public string CurrentDegree { get; set; } = default!;
        [Required] public int ServiceInYears { get; set; }
        [Required, Range(350000, double.MaxValue)] public decimal BaseSalary { get; set; }
        [Required] public Guid DepartmentId { get; set; }
    }
}
