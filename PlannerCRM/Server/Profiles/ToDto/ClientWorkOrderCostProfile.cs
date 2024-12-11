namespace PlannerCRM.Server.Profiles.ToDto;

public class ClientWorkOrderCostDtoProfile : Profile
{
    public ClientWorkOrderCostDtoProfile()
    {
        CreateMap<ClientWorkOrder, ClientWorkOrderCostDto>();
    }
}
