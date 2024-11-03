namespace PlannerCRM.Server.Profiles;

public class ActivityWorkTimeProfile : Profile
{
    public ActivityWorkTimeProfile()
    {
        CreateMap<ActivityWorkTime, ActivityWorkTimeDto>();
    }
}
