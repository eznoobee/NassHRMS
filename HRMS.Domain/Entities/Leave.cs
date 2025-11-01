using HRMS.Domain.Common;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;

namespace HRMS.Domain.Entities
{
    public class Leave : BaseEntity
    {
        private readonly List<LeaveLog> _logs = new();
        public IReadOnlyCollection<LeaveLog> Logs => _logs.AsReadOnly();

        public LeaveType? Type { get; private set; }
        public LeaveStatus Status { get; private set; } = LeaveStatus.Creating;
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string? LeaveReason { get; private set; }
        public string? RejectionReason { get; private set; }
        public Guid EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = default!;

        private Leave() { } 
        public Leave(string createdby, Guid employeeId, LeaveType? type = null, DateTime? startDate = null, DateTime? endDate = null, string? leaveReason = null)
        {
            LogChange("Account Created", "", "", createdby);
            EmployeeId = employeeId;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            LeaveReason = leaveReason;
            Status = LeaveStatus.Creating;
        }

        private void LogChange(string field, string? oldValue, string? newValue, string changedBy)
        {
            if (oldValue == newValue) return;
            _logs.Add(new LeaveLog(field, oldValue ?? "", newValue ?? "", changedBy));
        }

        public void Submit(string submittedBy)
        {
            if (Status != LeaveStatus.Creating)
                throw new DomainValidationException("Only newly created leaves can be submitted.");

            if (StartDate == null || EndDate == null || Type == null || LeaveReason == null)
                throw new DomainValidationException("Leave Type, StartDate, EndDate, and LeaveReason are required to submit a leave.");

            if (StartDate > EndDate)
                throw new DomainValidationException("Start date cannot be after end date.");

            var old = Status;
            Status = LeaveStatus.Pending;
            LogChange(nameof(Status), old.ToString(), Status.ToString(), submittedBy);
        }

        public void Approve(string approvedBy)
        {
            if (Status != LeaveStatus.Pending)
                throw new DomainValidationException("Only pending leaves can be approved.");

            var old = Status;
            Status = LeaveStatus.Accepted;
            LogChange(nameof(Status), old.ToString(), Status.ToString(), approvedBy);
        }

        public void Reject(string rejectionReason, string rejectedBy)
        {
            if (string.IsNullOrWhiteSpace(rejectionReason))
                throw new DomainValidationException("Rejection reason required.");
            if (Status != LeaveStatus.Pending)
                throw new DomainValidationException("Only pending leaves can be rejected.");

            var old = Status;
            Status = LeaveStatus.Rejected;
            RejectionReason = rejectionReason;
            LogChange(nameof(RejectionReason), null, rejectionReason, rejectedBy);
            LogChange(nameof(Status), old.ToString(), Status.ToString(), rejectedBy);
        }

        public void UpdateDates(DateTime start, DateTime end, string changedBy)
        {
            if (start > end)
                throw new DomainValidationException("Start date cannot be after end date.");

            var oldStart = StartDate?.ToString("yyyy-MM-dd");
            var oldEnd = EndDate?.ToString("yyyy-MM-dd");
            StartDate = start;
            EndDate = end;

            LogChange(nameof(StartDate), oldStart, start.ToString("yyyy-MM-dd"), changedBy);
            LogChange(nameof(EndDate), oldEnd, end.ToString("yyyy-MM-dd"), changedBy);
        }

        public void UpdateType(LeaveType? newType, string changedBy)
        {
            var old = Type?.ToString();
            Type = newType;
            LogChange(nameof(Type), old, newType?.ToString(), changedBy);
        }

        public void UpdateLeaveReason(string? leaveReason, string changedBy)
        {
            var old = LeaveReason;
            LeaveReason = leaveReason;
            LogChange(nameof(LeaveReason), old, leaveReason, changedBy);
        }

        public void ClearLogs() => _logs.Clear();
    }
}
