

namespace PlannerCRM.Server.Profiles;

public class ActivityProfile : Profile
{
    public ActivityProfile() 
    {
        CreateMap<Activity, ActivityDto>();
    }
}
