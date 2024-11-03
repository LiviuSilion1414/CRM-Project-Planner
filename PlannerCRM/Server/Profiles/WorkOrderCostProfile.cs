namespace PlannerCRM.Server.Profiles;

public class WorkOrderCostProfile : Profile
{
    public WorkOrderCostProfile()
    {
        CreateMap<WorkOrderCost, WorkOrderCostDto>();
    }
}
