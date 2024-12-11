namespace PlannerCRM.Server.Profiles.ToDto;

public class WorkTimeProfile : Profile
{
    public WorkTimeProfile()
    {
        CreateMap<WorkTime, WorkTimeDto>();
    }
}
