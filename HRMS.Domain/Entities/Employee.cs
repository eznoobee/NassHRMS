using HRMS.Domain.Common;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography;

namespace HRMS.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string Username { get; set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        public string? Email { get; private set; }
        public string PasswordHash { get; private set; } = default!;
        public Role Role { get; private set; } = Role.Employee;

        public string FirstName { get; private set; } = default!;
        public string FatherName { get; private set; } = default!;
        public string? GrandFatherName { get; private set; }
        public string? LastName { get; private set; }
        public string FullName { get; private set; } = default!;

        public Gender Gender { get; private set; }
        public DegreeType CurrentDegree { get; private set; }
        public int ServiceInYears { get; private set; }
        public decimal BaseSalary { get; private set; }

        public Guid DepartmentId { get; private set; }
        public Department Department { get; private set; } = default!;
        public Department? ManagedDepartment { get; private set; }
        public ICollection<Leave> Leaves { get; private set; } = new List<Leave>();

        private readonly List<EmployeeLog> _logs = new();
        public IReadOnlyCollection<EmployeeLog> Logs => _logs.AsReadOnly();

        private Employee() { }

        public Employee(
            string createdBy,
            string username,
            string phoneNumber,
            string? email,
            string passwordHash,
            Role role,
            string firstName,
            string fatherName,
            string? grandFatherName,
            string? lastName,
            Gender gender,
            DegreeType degree,
            int serviceInYears,
            decimal baseSalary,
            Guid departmentId)
        {
            LogChange("Employee Created", "", "", createdBy);
            Username = username;
            PhoneNumber = phoneNumber;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            FirstName = firstName;
            FatherName = fatherName;
            GrandFatherName = grandFatherName;
            LastName = lastName;
            Gender = gender;
            CurrentDegree = degree;
            ServiceInYears = serviceInYears;
            BaseSalary = baseSalary;
            DepartmentId = departmentId;
        }

        private void LogChange(string field, string? oldValue, string? newValue, string? changedBy)
        {
            if (oldValue == newValue) return;

            _logs.Add(new EmployeeLog(
                field,
                oldValue ?? "",
                newValue ?? "",
                string.IsNullOrWhiteSpace(changedBy) ? "System" : changedBy
            ));
        }

        private static void EnsureNotNullOrWhiteSpace(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException($"{fieldName} cannot be null or empty.");
        }

        public void UpdateUsername(string username, string changedBy)
        {
            EnsureNotNullOrWhiteSpace(username, nameof(Username));
            var old = Username;
            Username = username;
            LogChange(nameof(Username), old, username, changedBy);
        }

        public void UpdatePhoneNumber(string phoneNumber, string changedBy)
        {
            EnsureNotNullOrWhiteSpace(phoneNumber, nameof(PhoneNumber));
            var old = PhoneNumber;
            PhoneNumber = phoneNumber;
            LogChange(nameof(PhoneNumber), old, phoneNumber, changedBy);
        }

        public void UpdateEmail(string? email, string changedBy)
        {
            var old = Email;
            Email = email;
            LogChange(nameof(Email), old, email, changedBy);
        }

        public void UpdatePassword(string newHash, string changedBy)
        {
            EnsureNotNullOrWhiteSpace(newHash, nameof(PasswordHash));
            var old = PasswordHash;
            PasswordHash = newHash;
            LogChange(nameof(PasswordHash), "********", "********", changedBy);
        }

        public void UpdateRole(Role role, string changedBy)
        {
            var old = Role;
            Role = role;
            LogChange(nameof(Role), old.ToString(), role.ToString(), changedBy);
        }


        public void UpdateFirstName(string firstName, string changedBy)
        {
            EnsureNotNullOrWhiteSpace(firstName, nameof(FirstName));
            var old = FirstName;
            FirstName = firstName;
            LogChange(nameof(FirstName), old, firstName, changedBy);
        }

        public void UpdateFatherName(string fatherName, string changedBy)
        {
            EnsureNotNullOrWhiteSpace(fatherName, nameof(FatherName));
            var old = FatherName;
            FatherName = fatherName;
            LogChange(nameof(FatherName), old, fatherName, changedBy);
        }

        public void UpdateGrandFatherName(string? grandFatherName, string changedBy)
        {
            var old = GrandFatherName;
            GrandFatherName = grandFatherName;
            LogChange(nameof(GrandFatherName), old, grandFatherName, changedBy);
        }

        public void UpdateLastName(string? lastName, string changedBy)
        {
            var old = LastName;
            LastName = lastName;
            LogChange(nameof(LastName), old, lastName, changedBy);
        }

        public void UpdateGender(Gender gender, string changedBy)
        {
            var old = Gender;
            Gender = gender;
            LogChange(nameof(Gender), old.ToString(), gender.ToString(), changedBy);
        }

        public void UpdateDegree(DegreeType degree, string changedBy)
        {
            var old = CurrentDegree;
            CurrentDegree = degree;
            LogChange(nameof(CurrentDegree), old.ToString(), degree.ToString(), changedBy);
        }

        public void UpdateServiceInYears(int years, string changedBy)
        {
            if (years < 0)
                throw new DomainValidationException("Service years cannot be negative.");

            var old = ServiceInYears;
            ServiceInYears = years;
            LogChange(nameof(ServiceInYears), old.ToString(), years.ToString(), changedBy);
        }

        public void UpdateBaseSalary(decimal baseSalary, string changedBy)
        {
            if (baseSalary <= 0)
                throw new DomainValidationException("Base salary must be positive.");

            var old = BaseSalary;
            BaseSalary = baseSalary;
            LogChange(nameof(BaseSalary), old.ToString("F2"), baseSalary.ToString("F2"), changedBy);
        }
        public void UpdateDepartmentId(Guid departmentId, string changedBy)
        {
            var old = DepartmentId;
            DepartmentId = departmentId;
            LogChange(nameof(DepartmentId), old.ToString(), departmentId.ToString(), changedBy);
        }
        public void UpdateDepartment(Department department, string changedBy)
        {
            var old = Department.Name;
            Department = department;
            LogChange(nameof(DepartmentId), old.ToString(), department.Name, changedBy);
        }
        public decimal CalculateAllowances()
        {
            return AllowanceRate.GetRate(CurrentDegree) * BaseSalary;
        }
        public decimal CalculateServiceBouns()
        {
            return (ServiceInYears / 4) * 100000;
        }
        public decimal CalculateTotalSalary()
        {
            var allowanceRate = AllowanceRate.GetRate(CurrentDegree);
            var allowance = BaseSalary * allowanceRate;
            var serviceBonus = (ServiceInYears / 4) * 100000;
            return BaseSalary + allowance + serviceBonus;
        }
        public void ClearLogs() => _logs.Clear();
    }
}
