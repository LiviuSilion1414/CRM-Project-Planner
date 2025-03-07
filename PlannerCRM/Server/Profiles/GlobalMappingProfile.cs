namespace PlannerCRM.Server.Profiles;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<Activity, ActivityDto>().PreserveReferences().ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(x => x.PhoneNumber, y => y.MapFrom(z => z.Phone))
            .ForMember(x => x.Password, y => y.MapFrom(z => z.PasswordHash))
            //.ForMember(x => x.EmployeeRoles, y => y.MapFrom(z => z.EmployeeRoles))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<FirmClient, FirmClientDto>().PreserveReferences().ReverseMap();

        CreateMap<RoleDto, Role>().PreserveReferences().ReverseMap();

        CreateMap<WorkOrder, WorkOrderDto>().PreserveReferences().ReverseMap();

        CreateMap<Employee, LoginRecoveryDto>().PreserveReferences().ReverseMap();
    }
}
