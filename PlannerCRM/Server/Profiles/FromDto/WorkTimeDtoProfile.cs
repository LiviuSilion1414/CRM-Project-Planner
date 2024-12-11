namespace PlannerCRM.Server.Profiles.FromDto;

public class WorkTimeDtoProfile : Profile
{
    public WorkTimeDtoProfile()
    {
        CreateMap<WorkTimeDto, WorkTime>();
    }
}
