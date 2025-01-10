namespace PlannerCRM.Server.Profiles;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, ActivityDto>().MaxDepth(1);
        CreateMap<ActivityDto, Activity>().MaxDepth(1);
    }
}
