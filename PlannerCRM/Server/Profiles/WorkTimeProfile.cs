namespace PlannerCRM.Server.Profiles;

public class WorkTimeProfile : Profile
{
    public WorkTimeProfile()
    {
        CreateMap<WorkTime, WorkTimeDto>().MaxDepth(1);
        CreateMap<WorkTimeDto, WorkTime>().MaxDepth(1);
    }
}
