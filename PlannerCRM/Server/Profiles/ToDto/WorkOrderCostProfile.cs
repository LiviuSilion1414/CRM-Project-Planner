namespace PlannerCRM.Server.Profiles.ToDto;

public class WorkOrderCostProfile : Profile
{
    public WorkOrderCostProfile()
    {
        CreateMap<WorkOrderCost, WorkOrderCostDto>();
    }
}
