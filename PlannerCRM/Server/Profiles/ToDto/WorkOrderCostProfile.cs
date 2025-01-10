namespace PlannerCRM.Server.Profiles.ToDto;

public class WorkOrderCostProfile : Profile
{
    public WorkOrderCostProfile()
    {
        CreateMap<WorkOrderCost, WorkOrderCostDto>().MaxDepth(1);
        CreateMap<WorkOrderCostDto, WorkOrderCost>().MaxDepth(1);
    }
}
