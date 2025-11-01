using HRMS.Domain.Common;

namespace HRMS.Domain.Entities
{
    public class DepartmentLog : BaseLog
    {
        public DepartmentLog(string field, string? oldValue, string? newValue, string changedBy)
            : base(field, oldValue, newValue, changedBy)
        {
        }
    }
}
