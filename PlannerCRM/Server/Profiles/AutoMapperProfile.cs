using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.fkIdFirmClientNavigation, opt => opt.MapFrom(src => src.FkIdWorkOrderNavigation.FkIdFirmClientNavigation))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .PreserveReferences().ReverseMap();

        CreateMap<EmployeesRole, EmployeesRolesDto>()
        .PreserveReferences().ReverseMap();

        CreateMap<FirmClient, FirmClientDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.isRemoveable, opt => opt.MapFrom(src => src.EmployeesRoles.Any()))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<WorkOrder, WorkOrderDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<WorkTime, WorkTimeDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, LoginRecoveryDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<EmployeeActivity, EmployeeActivityDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<MenuRole, MenuRoleDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Menu, MenuDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Setting, SettingDto>()
            .PreserveReferences()
            .ReverseMap();
    }
}