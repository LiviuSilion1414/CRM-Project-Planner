namespace PlannerCRM.Server.Profiles.FromDto;

public class ActivityDtoProfile : Profile
{
    public ActivityDtoProfile()
    {
        CreateMap<ActivityDto, Activity>();
    }
}
