using HRMS.Domain.Common;

namespace HRMS.Domain.Entities
{
    public class LeaveLog : BaseLog
    {
        public LeaveLog(string field, string? oldValue, string? newValue, string changedBy)
            : base(field, oldValue, newValue, changedBy)
        {
        }
    }
}
