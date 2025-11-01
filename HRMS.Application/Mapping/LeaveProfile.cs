using AutoMapper;
using HRMS.Domain.Entities;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.DTOs.Department;
using HRMS.Application.DTOs.Leave;

namespace HRMS.Application.Mapping
{
    public class LeaveProfile : Profile
    {
        public LeaveProfile()
        {
            CreateMap<Leave, LeaveDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Employee.Username))
                .ForMember(dest => dest.EmployeePhone, opt => opt.MapFrom(src => src.Employee.PhoneNumber))
                .ForMember(dest => dest.EmployeeEmail, opt => opt.MapFrom(src => src.Employee.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Departmentname, opt => opt.MapFrom(src => src.Employee.Department.Name));
        }
    }
}
