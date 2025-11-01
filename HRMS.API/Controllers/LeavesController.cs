using HRMS.Application.DTOs.Leave;
using HRMS.Application.Interfaces.Services;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeavesController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateLeave()
        {
            var result = await _leaveService.CreateLeaveAsync();
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPost("{leaveId:Guid}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> SubmitLeave(Guid leaveId, UpdateLeaveDto dto)
        {
            var update = await _leaveService.UpdateLeaveAsync(leaveId,dto);
            if (!update.Success) return BadRequest(update.Message);

            var result = await _leaveService.SubmitLeaveAsync(leaveId);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("{id:Guid}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveLeave(Guid id)
        {
            var result = await _leaveService.ApproveLeaveAsync(id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id:Guid}/reject")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectLeave(Guid id, string reason)
        {
            var result = await _leaveService.RejectLeaveAsync(id,reason);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "HR,Manager")]
        public async Task<IActionResult> GetAll([FromQuery] FilterLeaveDto dto)
        {
            var result = await _leaveService.GetPaginatedLeavesAsync(dto);
            if (result == null)
                return NotFound("Leaves not found.");

            return Ok(result);
        }

        [HttpGet("self")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeLeaves()
        {
            var result = await _leaveService.GetLeavesByEmployeeAsync();
            return Ok(result.Data);
        }
    }
}
