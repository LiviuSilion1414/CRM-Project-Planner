namespace PlannerCRM.Server.Profiles;

public class WorkOrderActivityProfile : Profile
{
    public WorkOrderActivityProfile()
    {
        CreateMap<WorkOrderActivity, WorkOrderActivityDto>();
    }
}
