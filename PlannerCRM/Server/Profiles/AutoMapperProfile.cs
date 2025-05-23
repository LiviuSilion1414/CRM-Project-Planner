using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Activity, ActivityDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Employee, EmployeeDto>()
            .PreserveReferences().ReverseMap();

        CreateMap<EmployeesRole, EmployeesRoleDto>()
        .PreserveReferences().ReverseMap();

        CreateMap<FirmClient, FirmClientDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<RoleDto, Role>()
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
    }
}