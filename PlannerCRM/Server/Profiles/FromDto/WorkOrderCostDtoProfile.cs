namespace PlannerCRM.Server.Profiles.FromDto;

public class WorkOrderCostDtoProfile : Profile
{
    public WorkOrderCostDtoProfile()
    {
        CreateMap<WorkOrderCostDto, WorkOrderCost>();
    }
}
