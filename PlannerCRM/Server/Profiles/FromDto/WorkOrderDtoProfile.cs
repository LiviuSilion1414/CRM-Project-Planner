namespace PlannerCRM.Server.Profiles.FromDto;

public class WorkOrderDtoProfile : Profile
{
    public WorkOrderDtoProfile()
    {
        CreateMap<WorkOrderDto, WorkOrder>();
    }
}
