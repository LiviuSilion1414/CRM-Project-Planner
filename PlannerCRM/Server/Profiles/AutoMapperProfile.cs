using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Activity, ActivityDto>()
            //.ForMember(dest => dest.employees, cfg => cfg.MapFrom(src => src.EmployeeActivities.Select(x => x.FkIdEmployeeNavigation)))
            //.ForMember(dest => dest.workOrder, cfg => cfg.MapFrom(src => src.FkIdWorkOrderNavigation))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.roles, cfg => cfg.MapFrom(src => src.EmployeesRoles.Select(x => new RoleDto() { id = x.FkIdRole, name= x.RoleName })))
            .ForMember(dest => dest.password, cfg => cfg.MapFrom(src => src.PasswordHash))
            .PreserveReferences().ReverseMap();

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
            .ForMember(dest => dest.activityId, cfg => cfg.MapFrom(src => src.FkIdActivity))
            .ForMember(dest => dest.employeeId, cfg => cfg.MapFrom(src => src.FkIdEmployee))
            .ForMember(dest => dest.activity, cfg => cfg.MapFrom(src => src.FkIdActivityNavigation))
            .ForMember(dest => dest.employee, cfg => cfg.MapFrom(src => src.FkIdEmployeeNavigation))
            .PreserveReferences()
            .ReverseMap();
    }
}