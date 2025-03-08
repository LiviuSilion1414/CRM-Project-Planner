namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>().PreserveReferences().ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .MaxDepth(1)
            .ForMember(x => x.phone, y => y.MapFrom(z => z.Phone))
            .ForMember(x => x.password, y => y.MapFrom(z => z.PasswordHash))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<FirmClient, FirmClientDto>()
            .MaxDepth(1)
            .PreserveReferences().ReverseMap();

        CreateMap<RoleDto, Role>()
            .MaxDepth(1)
            .PreserveReferences()
            .ReverseMap();

        CreateMap<WorkOrder, WorkOrderDto>()
            .MaxDepth(1)
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, LoginRecoveryDto>()
            .MaxDepth(1)
            .PreserveReferences()
            .ReverseMap();
    }
}
