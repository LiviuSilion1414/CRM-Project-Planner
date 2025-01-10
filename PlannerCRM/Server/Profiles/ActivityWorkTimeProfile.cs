namespace PlannerCRM.Server.Profiles;

public class ActivityWorkTimeProfile : Profile
{
    public ActivityWorkTimeProfile()
    {
        CreateMap<ActivityWorkTime, ActivityWorkTimeDto>().MaxDepth(1);
        CreateMap<ActivityWorkTimeDto, ActivityWorkTime>().MaxDepth(1);
    }
}
