namespace PlannerCRM.Server.Profiles.ToDto;

public class ClientWorkOrderProfile : Profile
{
    public ClientWorkOrderProfile()
    {
        CreateMap<ClientWorkOrder, ClientWorkOrderDto>().MaxDepth(1);
        CreateMap<ClientWorkOrderDto, ClientWorkOrder>().MaxDepth(1);
    }
}
