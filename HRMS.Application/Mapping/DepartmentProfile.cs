using AutoMapper;
using HRMS.Application.DTOs.Department;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null));

            CreateMap<CreateDepartmentDto, Department>();
        }
    }
}
