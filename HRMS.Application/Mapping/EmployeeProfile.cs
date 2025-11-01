using AutoMapper;
using HRMS.Application.DTOs.Employee;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;

namespace HRMS.Application.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, CreateEmployeeDto>()
                .ForMember(dest => dest.CurrentDegree, opt => opt.MapFrom(src => src.CurrentDegree.ToString()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(dest => dest.CurrentDegree,
                   opt => opt.MapFrom(src => Enum.Parse<Gender>(src.CurrentDegree, true)))
                .ForMember(dest => dest.Gender,
                   opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)))
                .ForMember(dest => dest.Role,
                   opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Role, true)));

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Allowances, opt => opt.MapFrom(src => src.CalculateAllowances()))
                .ForMember(dest => dest.ServiceBouns, opt => opt.MapFrom(src => src.CalculateServiceBouns()))
                .ForMember(dest => dest.TotalSalary, opt => opt.MapFrom(src => src.CalculateTotalSalary()))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.CurrentDegree, opt => opt.MapFrom(src => src.CurrentDegree.ToString()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.MaanagerName,opt => opt.MapFrom(src => src.Department.Manager != null ? src.Department.Manager.FullName : string.Empty));

        }
    }
}
