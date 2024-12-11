namespace PlannerCRM.Server.Profiles.FromDto;

public class ClientWorkOrderDtoProfile : Profile
{
    public ClientWorkOrderDtoProfile()
    {
        CreateMap<ClientWorkOrderDto, ClientWorkOrder>();
    }
}
