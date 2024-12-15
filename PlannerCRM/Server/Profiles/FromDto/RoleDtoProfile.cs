namespace PlannerCRM.Server.Profiles.FromDto;

public class RoleDtoProfile : Profile
{
    public RoleDtoProfile()
    {
        CreateMap<RoleDto, Role>();
    }
}
