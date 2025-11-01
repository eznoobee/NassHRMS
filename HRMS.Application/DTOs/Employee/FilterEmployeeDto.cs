namespace HRMS.Application.DTOs.Employee
{
    public class FilterEmployeeDto
    {
        public string? SearchKeyword { get; set; }
        public string? DepartmentName { get; set; }
        public string? Degree { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
