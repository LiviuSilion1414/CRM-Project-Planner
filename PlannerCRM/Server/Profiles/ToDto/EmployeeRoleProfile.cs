namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeRoleProfile : Profile
{
    public EmployeeRoleProfile()
    {
        CreateMap<EmployeeRole, EmployeeRoleDto>().MaxDepth(1);
        CreateMap<EmployeeRoleDto, EmployeeRole>().MaxDepth(1);
    }
}
