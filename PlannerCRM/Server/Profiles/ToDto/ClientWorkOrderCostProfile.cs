namespace PlannerCRM.Server.Profiles.ToDto;

public class ClientWorkOrderCostDtoProfile : Profile
{
    public ClientWorkOrderCostDtoProfile()
    {
        CreateMap<ClientWorkOrder, ClientWorkOrderCostDto>().MaxDepth(1);
        CreateMap<ClientWorkOrderDto, ClientWorkOrderCost>().MaxDepth(1);
    }
}
