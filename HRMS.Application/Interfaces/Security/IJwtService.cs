namespace HRMS.Application.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateToken(Guid employeeId, string role, string? email = null, string? phone = null, string? username = null);
    }
}
