namespace PlannerCRM.Server.Profiles.ToDto;

public class WorkOrderActivityProfile : Profile
{
    public WorkOrderActivityProfile()
    {
        CreateMap<WorkOrderActivity, WorkOrderActivityDto>();
    }
}
