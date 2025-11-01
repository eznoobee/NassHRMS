using HRMS.Application.DTOs.Department;
using HRMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "HR")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            var result = await _departmentService.CreateDepartmentAsync(dto);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();
            return Ok(result.Data);
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _departmentService.GetDepartmentByNameAsync(name);
            if (!result.Success) return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
