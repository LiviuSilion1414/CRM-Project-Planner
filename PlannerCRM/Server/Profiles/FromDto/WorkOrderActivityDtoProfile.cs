namespace PlannerCRM.Server.Profiles.FromDto;

public class WorkOrderActivityDtoProfile : Profile
{
    public WorkOrderActivityDtoProfile()
    {
        CreateMap<WorkOrderActivityDto, WorkOrderActivity>();
    }
}
