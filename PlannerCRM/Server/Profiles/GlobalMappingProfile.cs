namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.roles, cfg => cfg.MapFrom(src => src.EmployeeRoles.Select(x => new RoleDto() { id = x.RoleId, roleName= x.RoleName } )))
            .PreserveReferences()
            .ReverseMap();

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