using HRMS.Application.Interfaces.Security;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace HRMS.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        private ClaimsPrincipal? User => _contextAccessor.HttpContext?.User;
        public Guid? GetUserId()
        {
            var subClaim =
                User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(subClaim, out var id) ? id : null;
        }
        public string GetUserName()
        {
            var username =
                User?.FindFirst(ClaimTypes.Name)?.Value ??
                User?.FindFirst("username")?.Value ??
                User?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value ??
                User?.Identity?.Name;

            return string.IsNullOrWhiteSpace(username) ? "System" : username;
        }
        public string GetUserRole()
        {
            var role =
                User?.FindFirst("role")?.Value ??
                User?.FindFirst(ClaimTypes.Role)?.Value;

            return string.IsNullOrWhiteSpace(role) ? "Unknown" : role;
        }
        public string? GetUserEmail() =>
            User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        public string? GetUserPhone() =>
            User?.FindFirst("phone_number")?.Value;
    }
}
