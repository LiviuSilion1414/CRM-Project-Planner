namespace PlannerCRM.Server.Profiles.ToDto;

public class FirmClientProfile : Profile
{
    public FirmClientProfile()
    {
        CreateMap<FirmClient, FirmClientDto>();
    }
}
