using AutoMapper;
using HRMS.Application.Common;
using HRMS.Application.DTOs.Leave;
using HRMS.Application.Interfaces.Persistence;
using HRMS.Application.Interfaces.Security;
using HRMS.Application.Interfaces.Services;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using System.Collections.Generic;

namespace HRMS.Application.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveService(
            IMapper mapper,
            ILeaveRepository leaveRepository,
            IEmployeeRepository employeeRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _leaveRepository = leaveRepository;
            _employeeRepository = employeeRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<LeaveDto>> CreateLeaveAsync()
        {
            var employeeId = _currentUserService.GetUserId();
            if (employeeId == null)
                return Result<LeaveDto>.Fail("Current user ID not found.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId.Value);
            if (employee == null)
                return Result<LeaveDto>.Fail("Employee not found.");

            var leave = new Leave(createdby: _currentUserService.GetUserName(), employeeId: employee.Id);
            await _leaveRepository.AddAsync(leave);

            var mapped = _mapper.Map<LeaveDto>(leave);
            return Result<LeaveDto>.Ok(mapped, "Leave request created successfully.");
        }

        public async Task<Result<bool>> UpdateLeaveAsync(Guid leaveId, UpdateLeaveDto dto)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);
            if (leave is null)
                return Result<bool>.Fail("Leave not found.");

            var currentUser = _currentUserService.GetUserName();

            if (dto.Type != null)
                leave.UpdateType(Enum.Parse<LeaveType>(dto.Type, true), currentUser);

            if (dto.StartDate.HasValue && dto.EndDate.HasValue)
                leave.UpdateDates(dto.StartDate.Value, dto.EndDate.Value, currentUser);

            if (!string.IsNullOrWhiteSpace(dto.LeaveReason))
                leave.UpdateLeaveReason(dto.LeaveReason, currentUser);

            await _leaveRepository.UpdateAsync(leave);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<LeaveDto>> SubmitLeaveAsync(Guid leaveId)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);
            if (leave == null)
                return Result<LeaveDto>.Fail("Leave not found.");

            leave.Submit(submittedBy: _currentUserService.GetUserName());
            await _leaveRepository.UpdateAsync(leave);

            var result = _mapper.Map<LeaveDto>(leave);
            return Result<LeaveDto>.Ok(result, "Leave request submitted successfully.");
        }
        public async Task<Result<IEnumerable<LeaveDto>>> GetLeavesByEmployeeAsync()
        {
            var employeeId = _currentUserService.GetUserId();

            if (employeeId == null)
                return Result<IEnumerable<LeaveDto>>.Fail("Unauthorized: user ID not found in token.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId.Value);
            if (employee == null)
                return Result<IEnumerable<LeaveDto>>.Fail("Employee not found.");

            var leaves = await _leaveRepository.GetByEmployeeIdAsync(employeeId.Value);
            var mapped = _mapper.Map<IEnumerable<LeaveDto>>(leaves);

            return Result<IEnumerable<LeaveDto>>.Ok(mapped);
        }

        public async Task<Result<LeaveDto>> ApproveLeaveAsync(Guid leaveId)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);
            if (leave == null)
                return Result<LeaveDto>.Fail("Leave not found.");

            if (leave.Status != LeaveStatus.Pending)
                return Result<LeaveDto>.Fail("Only pending leaves can be approved.");

            leave.Approve(_currentUserService.GetUserName());
            await _leaveRepository.UpdateAsync(leave);

            var result = _mapper.Map<LeaveDto>(leave);
            return Result<LeaveDto>.Ok(result, "Leave approved successfully.");
        }

        public async Task<Result<LeaveDto>> RejectLeaveAsync(Guid leaveId, string reason)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);
            if (leave == null)
                return Result<LeaveDto>.Fail("Leave not found.");

            if (leave.Status != LeaveStatus.Pending)
                return Result<LeaveDto>.Fail("Only pending leaves can be rejected.");

            if (string.IsNullOrWhiteSpace(reason))
                return Result<LeaveDto>.Fail("A rejection reason is required.");

            leave.Reject(reason, _currentUserService.GetUserName());
            await _leaveRepository.UpdateAsync(leave);

            var result = _mapper.Map<LeaveDto>(leave);
            return Result<LeaveDto>.Ok(result, "Leave rejected successfully.");
        }

        public async Task<PaginatedResult<LeaveDto>> GetPaginatedLeavesAsync(FilterLeaveDto dto)
        {
            LeaveStatus? status = null;

            if (!string.IsNullOrWhiteSpace(dto.LeaveStatus) &&
                Enum.TryParse(dto.LeaveStatus, true, out LeaveStatus parsedStatus))
            {
                status = parsedStatus;
            }

            var (leaves, totalCount) = await _leaveRepository
                .GetPaginatedAsync(dto.PageNumber, dto.PageSize, status, dto.SearchKeyword);

            var leavesDtos = _mapper.Map<IEnumerable<LeaveDto>>(leaves);
            return new PaginatedResult<LeaveDto>(leavesDtos, totalCount, dto.PageNumber, dto.PageSize);
        }

    }
}
