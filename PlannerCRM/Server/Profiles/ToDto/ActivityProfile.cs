namespace PlannerCRM.Server.Profiles.ToDto;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, ActivityDto>();
    }
}
