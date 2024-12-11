namespace PlannerCRM.Server.Profiles.ToDto;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>();
    }
}
