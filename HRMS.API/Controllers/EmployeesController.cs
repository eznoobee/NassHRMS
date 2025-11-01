using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces.Services;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("self")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetByIdAsync()
        {
            var result = await _employeeService.GetByIdAsync();
            if (result == null)
                return NotFound("Employee not found.");

            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            if (result == null)
                return NotFound("Employee not found.");

            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetAll([FromQuery] FilterEmployeeDto employeeFilterDto)
        {
            var result = await _employeeService.GetPaginatedEmployeesAsync(employeeFilterDto);
            if (result == null)
                return NotFound("Employees not found.");

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Register([FromBody] CreateEmployeeDto dto)
        {
            var result = await _employeeService.CreateEmployeeAsync(dto);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto dto)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, dto);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result.Success) return NotFound(result.Message);
            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}
