namespace PlannerCRM.Server.Profiles;

public class WorkOrderProfile : Profile
{
    public WorkOrderProfile()
    {
        CreateMap<WorkOrder, WorkOrderDto>().MaxDepth(1);
        CreateMap<WorkOrderDto, WorkOrder>().MaxDepth(1);
    }
}
