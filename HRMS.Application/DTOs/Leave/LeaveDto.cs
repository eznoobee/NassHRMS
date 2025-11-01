namespace HRMS.Application.DTOs.Leave
{
    public class LeaveDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? LeaveReason { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeName { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string EmployeePhone { get; set; } = default!;
        public string? EmployeeEmail { get; set; }
        public string Departmentname { get; set; } = default!;
    }
}
