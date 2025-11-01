using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Persistence
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Leave> Leaves => Set<Leave>();

        public DbSet<EmployeeLog> EmployeeLogs => Set<EmployeeLog>();
        public DbSet<LeaveLog> LeaveLogs => Set<LeaveLog>();
        public DbSet<DepartmentLog> DepartmentLogs => Set<DepartmentLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRMSDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
