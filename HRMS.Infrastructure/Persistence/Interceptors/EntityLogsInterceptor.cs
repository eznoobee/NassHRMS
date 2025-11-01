using HRMS.Domain.Common;
using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HRMS.Infrastructure.Persistence.Interceptors
{
    public class EntityLogsInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var entitiesWithLogs = context.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is Employee emp && emp.Logs.Any() ||
                    e.Entity is Department dept && dept.Logs.Any() ||
                    e.Entity is Leave leave && leave.Logs.Any())
                .Select(e => e.Entity)
                .ToList();

            foreach (var entity in entitiesWithLogs)
            {
                switch (entity)
                {
                    case Employee employee:
                        await PersistLogsAsync(context, employee.Logs, context.Set<EmployeeLog>(), employee.ClearLogs, cancellationToken);
                        break;

                    case Department department:
                        await PersistLogsAsync(context, department.Logs, context.Set<DepartmentLog>(), department.ClearLogs, cancellationToken);
                        break;

                    case Leave leave:
                        await PersistLogsAsync(context, leave.Logs, context.Set<LeaveLog>(), leave.ClearLogs, cancellationToken);
                        break;
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private static async Task PersistLogsAsync<TLog>(
            DbContext context,
            IReadOnlyCollection<TLog> logs,
            DbSet<TLog> dbSet,
            Action clearLogs,
            CancellationToken cancellationToken)
            where TLog : BaseLog
        {
            if (logs.Count == 0)
                return;

            await dbSet.AddRangeAsync(logs, cancellationToken);
            clearLogs();
        }
    }
}
