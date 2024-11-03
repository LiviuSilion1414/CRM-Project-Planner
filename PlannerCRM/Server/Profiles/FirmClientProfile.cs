namespace PlannerCRM.Server.Profiles;

public class FirmClientProfile : Profile
{
    public FirmClientProfile()
    {
        CreateMap<FirmClient, FirmClientDto>();
    }
}
