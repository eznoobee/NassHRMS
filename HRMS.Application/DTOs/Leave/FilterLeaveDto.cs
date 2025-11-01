namespace HRMS.Application.DTOs.Leave
{
    public class FilterLeaveDto
    {
        public string? SearchKeyword { get; set; }
        public string? LeaveStatus { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
