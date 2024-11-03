namespace PlannerCRM.Server.Profiles;

public class WorkTimeProfile : Profile
{
    public WorkTimeProfile()
    {
        CreateMap<WorkTime, WorkTimeDto>();
    }
}
