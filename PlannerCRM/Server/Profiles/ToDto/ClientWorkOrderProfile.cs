namespace PlannerCRM.Server.Profiles.ToDto;

public class ClientWorkOrderProfile : Profile
{
    public ClientWorkOrderProfile()
    {
        CreateMap<ClientWorkOrder, ClientWorkOrderDto>();
    }
}
