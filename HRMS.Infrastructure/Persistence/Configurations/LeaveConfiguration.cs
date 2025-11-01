using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations
{
    public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.ToTable("Leaves");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(l => l.Type)
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(l => l.EmployeeId)
                   .IsRequired();

            builder.Property(l => l.Status)
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(l => l.LeaveReason)
                   .HasMaxLength(250);

            builder.Property(l => l.RejectionReason)
                   .HasMaxLength(250);

            builder.Property(l => l.StartDate);

            builder.Property(l => l.EndDate);

            builder.HasOne(l => l.Employee)
                   .WithMany(e => e.Leaves)
                   .HasForeignKey(l => l.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
