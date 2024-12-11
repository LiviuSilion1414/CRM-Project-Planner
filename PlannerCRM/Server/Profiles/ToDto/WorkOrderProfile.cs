namespace PlannerCRM.Server.Profiles.ToDto;

public class WorkOrderProfile : Profile
{
    public WorkOrderProfile()
    {
        CreateMap<WorkOrder, WorkOrderDto>();
    }
}
