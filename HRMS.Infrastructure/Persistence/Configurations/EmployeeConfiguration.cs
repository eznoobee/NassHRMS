using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd();


            builder.Property(e => e.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(e => e.Username)
                   .IsUnique();

            builder.Property(e => e.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(e => e.PhoneNumber)
                   .IsUnique();

            builder.Property(e => e.Email)
                   .HasMaxLength(120);

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.Role)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();


            builder.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.FatherName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.GrandFatherName)
                   .HasMaxLength(50);

            builder.Property(e => e.LastName)
                   .HasMaxLength(50);

            builder.Property(e => e.FullName)
                   .HasComputedColumnSql(
                       "trim(\"FirstName\" || ' ' || \"FatherName\" || ' ' || coalesce(\"GrandFatherName\", '') || ' ' || coalesce(\"LastName\", ''))",
                       stored: true
                   )
                   .HasMaxLength(200);


            builder.Property(e => e.BaseSalary)
                   .IsRequired();

            builder.Property(e => e.ServiceInYears)
                   .IsRequired();

            builder.Property(e => e.CurrentDegree)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.Property(e => e.Gender)
                   .HasConversion<string>()
                   .HasMaxLength(10);



            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Leaves)
                   .WithOne(l => l.Employee)
                   .HasForeignKey(l => l.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
