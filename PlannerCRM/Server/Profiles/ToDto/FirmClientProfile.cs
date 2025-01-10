namespace PlannerCRM.Server.Profiles.ToDto;

public class FirmClientProfile : Profile
{
    public FirmClientProfile()
    {
        CreateMap<FirmClient, FirmClientDto>().MaxDepth(1);
        CreateMap<FirmClientDto, FirmClient>().MaxDepth(1);
    }
}
