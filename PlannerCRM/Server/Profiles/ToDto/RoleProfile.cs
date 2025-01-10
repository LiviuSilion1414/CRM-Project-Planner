namespace PlannerCRM.Server.Profiles.ToDto;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>().MaxDepth(1);
        CreateMap<RoleDto, Role>().MaxDepth(1);
    }
}
