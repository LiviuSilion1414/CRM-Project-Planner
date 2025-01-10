namespace PlannerCRM.Server.Profiles.ToDto;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, ActivityDto>().MaxDepth(1);
        CreateMap<ActivityDto, Activity>().MaxDepth(1);
    }
}
