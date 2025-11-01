namespace HRMS.Application.DTOs.Department
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? ManagerName { get; set; }
        public int EmployeeCount { get; set; }
    }
}
