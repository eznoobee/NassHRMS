using HRMS.Application.Interfaces.Persistence;
using HRMS.Application.Interfaces.Security;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Services;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Persistence.Interceptors;
using HRMS.Infrastructure.Repositories;
using HRMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HRMSDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<EntityLogsInterceptor>();

                options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(interceptor);
            });

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ILeaveService, LeaveService>();

            services.AddScoped<EntityLogsInterceptor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
