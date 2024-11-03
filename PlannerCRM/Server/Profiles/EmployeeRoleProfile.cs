namespace PlannerCRM.Server.Profiles;

public class EmployeeRoleProfile : Profile
{
    public EmployeeRoleProfile()
    {
        CreateMap<EmployeeRole, EmployeeRoleDto>();
    }
}
