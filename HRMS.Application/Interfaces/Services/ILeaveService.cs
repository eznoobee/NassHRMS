using HRMS.Application.Common;
using HRMS.Application.DTOs.Leave;

namespace HRMS.Application.Interfaces.Services
{
    public interface ILeaveService
    {
        Task<Result<LeaveDto>> CreateLeaveAsync();
        Task<Result<LeaveDto>> SubmitLeaveAsync(Guid leaveId);
        Task<Result<bool>> UpdateLeaveAsync(Guid leaveId, UpdateLeaveDto dto);
        Task<Result<LeaveDto>> ApproveLeaveAsync(Guid leaveId);
        Task<Result<LeaveDto>> RejectLeaveAsync(Guid leaveId, string reason);
        Task<Result<IEnumerable<LeaveDto>>> GetLeavesByEmployeeAsync();
        Task<PaginatedResult<LeaveDto>> GetPaginatedLeavesAsync(FilterLeaveDto dto);
    }
}
