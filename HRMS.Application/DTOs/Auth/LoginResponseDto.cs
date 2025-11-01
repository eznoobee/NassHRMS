using HRMS.Application.DTOs.Employee;

namespace HRMS.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Message { get; set; } = "Login successful";
        public string Token { get; set; } = default!;
        public DateTime Expiration { get; set; }
    }
}
