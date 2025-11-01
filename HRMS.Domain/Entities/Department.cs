using HRMS.Domain.Common;
using HRMS.Domain.Exceptions;

namespace HRMS.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public Guid? ManagerId { get; private set; }
        public Employee? Manager { get; private set; }
        public ICollection<Employee> Employees { get; private set; } = [];

        private readonly List<DepartmentLog> _logs = [];
        public IReadOnlyCollection<DepartmentLog> Logs => _logs.AsReadOnly();

        private Department() { }
        public Department(string createdby, string name, string? description,Employee? manager)
        {
            LogChange("Department Created", "", "", createdby);
            Name = name;
            Description = description;
            Manager = manager;
            ManagerId = manager?.Id;
        }
        private static void EnsureNotNullOrWhiteSpace(string? value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException($"{field} cannot be null or empty.");
        }
        private void LogChange(string field, string? oldValue, string? newValue, string changedBy)
        {
            if (oldValue == newValue) return;
            _logs.Add(new DepartmentLog(
                field,
                oldValue ?? "",
                newValue ?? "",
                string.IsNullOrWhiteSpace(changedBy) ? "System" : changedBy
            ));
        }
        public void UpdateName(string newName, string changedBy)
        {
            EnsureNotNullOrWhiteSpace(newName, nameof(Name));
            var old = Name;
            Name = newName;
            LogChange(nameof(Name), old, newName, changedBy);
        }
        public void UpdateDescription(string? newDesc, string changedBy)
        {
            var old = Description;
            Description = newDesc;
            LogChange(nameof(Description), old, newDesc, changedBy);
        }
        public void UpdateManager(Employee newManager, string changedBy)
        {
            var old = ManagerId?.ToString();
            ManagerId = newManager.Id;
            Manager = newManager;
            LogChange(nameof(ManagerId), old, newManager.Id.ToString(), changedBy);
        }
        public void ClearLogs() => _logs.Clear();
        public void AddEmployee(Employee employee)
        {
            if (!Employees.Contains(employee))
                Employees.Add(employee);
        }
    }
}
