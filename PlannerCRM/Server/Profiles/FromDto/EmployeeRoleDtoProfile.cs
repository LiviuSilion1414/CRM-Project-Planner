namespace PlannerCRM.Server.Profiles.FromDto;

public class EmployeeRoleDtoProfile : Profile
{
    public EmployeeRoleDtoProfile()
    {
        CreateMap<EmployeeRoleDto, EmployeeRole>();
    }
}
