namespace HRMS.Application.Interfaces.Security
{
    public interface ICurrentUserService
    {
        string GetUserName();
        string GetUserRole();
        Guid? GetUserId();
    }
}
