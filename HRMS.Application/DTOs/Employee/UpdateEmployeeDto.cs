using HRMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public string? Username { get; set; }
        [Phone] public string? PhoneNumber { get; set; }
        [EmailAddress] public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? FatherName { get; set; }
        public string? GrandFatherName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? CurrentDegree { get; set; }
        public int? ServiceInYears { get; set; }
        [Range(350000, double.MaxValue)] public decimal? BaseSalary { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
