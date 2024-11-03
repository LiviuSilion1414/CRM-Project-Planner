namespace PlannerCRM.Server.Profiles;

public class WorkOrderProfile : Profile
{
    public WorkOrderProfile()
    {
        CreateMap<WorkOrder, WorkOrderDto>();
    }
}
