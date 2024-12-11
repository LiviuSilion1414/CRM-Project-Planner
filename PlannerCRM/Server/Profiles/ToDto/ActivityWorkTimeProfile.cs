namespace PlannerCRM.Server.Profiles.ToDto;

public class ActivityWorkTimeProfile : Profile
{
    public ActivityWorkTimeProfile()
    {
        CreateMap<ActivityWorkTime, ActivityWorkTimeDto>();
    }
}
