namespace HRMS.Domain.Common
{
    public abstract class BaseLog : BaseEntity
    {
        public string Field { get; protected set; } = default!;
        public string? OldValue { get; protected set; }
        public string? NewValue { get; protected set; }
        public string ChangedBy { get; protected set; } = default!;
        public DateTime ChangedAt { get; protected set; } = DateTime.UtcNow;

        protected BaseLog() { }

        protected BaseLog(string field, string? oldValue, string? newValue, string changedBy)
        {
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedBy = changedBy;
            ChangedAt = DateTime.UtcNow;
        }
    }
}
