using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.employees, cfg => cfg.MapFrom(src => src.EmployeeActivities.Select(x => x.FkIdEmployeeNavigation)))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.roles, cfg => cfg.MapFrom(src => src.EmployeesRoles.Select(x => new RoleDto() { id = x.FkIdRole/*, name= x.RoleName*/ })))
            .ForMember(dest => dest.password, cfg => cfg.Ignore())
            .PreserveReferences();

        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.EmployeesRoles, cfg => cfg.MapFrom(src => src.roles.Select(x => new EmployeesRole() { FkIdRole = x.id/*, RoleName = x.name*/ })))
            .ForMember(dest => dest.PasswordHash, cfg => cfg.MapFrom(src => src.password))
            .PreserveReferences();

        CreateMap<FirmClient, FirmClientDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<RoleDto, Role>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<WorkOrder, WorkOrderDto>()
            .ForMember(dest => dest.firmClient, cfg => cfg.MapFrom(src => src.FkIdFirmClientNavigation))
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
    }
}