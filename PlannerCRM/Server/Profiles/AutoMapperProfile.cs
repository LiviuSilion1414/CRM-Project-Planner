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

        CreateMap<EmployeesRole, EmployeesRolesDto>()
        .PreserveReferences().ReverseMap();

        CreateMap<FirmClient, FirmClientDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Role, RoleDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<GroupRole, GroupRoleDto>()
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

        CreateMap<SystemLog, SystemLogDto>()
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Project, ProjectDto>()
            .PreserveReferences()
            .ReverseMap();
    }
}