namespace PlannerCRM.Server.Profiles.ToDto;

public class WorkOrderActivityProfile : Profile
{
    public WorkOrderActivityProfile()
    {
        CreateMap<WorkOrderActivity, WorkOrderActivityDto>().MaxDepth(1);
        CreateMap<WorkOrderActivityDto, WorkOrderActivity>().MaxDepth(1);
    }
}
