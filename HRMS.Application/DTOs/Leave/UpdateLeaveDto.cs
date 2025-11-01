namespace HRMS.Application.DTOs.Leave
{
    public class UpdateLeaveDto
    {
        public string? LeaveReason { get; set; } = default!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Type { get; set; } = default!;
    }
}
