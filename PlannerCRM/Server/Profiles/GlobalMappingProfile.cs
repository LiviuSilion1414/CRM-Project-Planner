namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.roles, cfg => cfg.MapFrom(src => src.EmployeeRoles.Select(x => new RoleDto() { id = x.RoleId, roleName= x.RoleName })))
            .ForMember(dest => dest.password, cfg => cfg.MapFrom(src => src.PasswordHash))
            .PreserveReferences();

        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.EmployeeRoles, cfg => cfg.MapFrom(src => src.roles.Select(x => new EmployeeRole() { RoleId = x.id, RoleName = x.roleName })))
            .ForMember(dest => dest.PasswordHash, cfg => cfg.MapFrom(src => src.password))
            .PreserveReferences();

        CreateMap<FirmClient, FirmClientDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<RoleDto, Role>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<WorkOrder, WorkOrderDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, LoginRecoveryDto>()
            .PreserveReferences()
            .ReverseMap();
    }
}