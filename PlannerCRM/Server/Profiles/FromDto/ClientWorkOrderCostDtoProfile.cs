namespace PlannerCRM.Server.Profiles.FromDto;

public class ClientWorkOrderCostDtoProfile : Profile
{
    public ClientWorkOrderCostDtoProfile()
    {
        CreateMap<ClientWorkOrderDto, ClientWorkOrderCost>();
    }
}
