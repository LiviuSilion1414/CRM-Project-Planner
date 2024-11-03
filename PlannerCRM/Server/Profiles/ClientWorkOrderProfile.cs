namespace PlannerCRM.Server.Profiles;

public class ClientWorkOrderProfile : Profile
{
    public ClientWorkOrderProfile()
    {
        CreateMap<ClientWorkOrder, ClientWorkOrderDto>();
    }
}
