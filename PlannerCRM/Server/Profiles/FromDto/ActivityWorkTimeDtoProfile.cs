namespace PlannerCRM.Server.Profiles.FromDto;

public class ActivityWorkTimeDtoProfile : Profile
{
    public ActivityWorkTimeDtoProfile()
    {
        CreateMap<ActivityWorkTimeDto, ActivityWorkTime>();
    }
}
