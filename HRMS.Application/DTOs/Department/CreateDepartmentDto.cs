using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        [Required] public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
