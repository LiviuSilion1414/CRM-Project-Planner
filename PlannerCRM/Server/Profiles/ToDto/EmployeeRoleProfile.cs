namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeRoleProfile : Profile
{
    public EmployeeRoleProfile()
    {
        CreateMap<EmployeeRole, EmployeeRoleDto>();
    }
}
