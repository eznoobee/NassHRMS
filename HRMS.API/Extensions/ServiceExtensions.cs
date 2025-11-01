using HRMS.Application.Mapping;
using HRMS.Application.Mappings;
using HRMS.Infrastructure.DependencyInjection;

namespace HRMS.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationAndInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(EmployeeProfile).Assembly);
            services.AddAutoMapper(typeof(DepartmentProfile).Assembly);
            services.AddAutoMapper(typeof(LeaveProfile).Assembly);

            services.AddInfrastructureServices(configuration);

            return services;
        }
    }
}
