namespace PlannerCRM.Server.Profiles;

public class ClientWorkOrderCostProfile : Profile
{
    public ClientWorkOrderCostProfile()
    {
        CreateMap<ClientWorkOrder, ClientWorkOrderCostDto>();
    }
}
