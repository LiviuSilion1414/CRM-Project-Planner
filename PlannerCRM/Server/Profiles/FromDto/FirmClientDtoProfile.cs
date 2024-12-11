namespace PlannerCRM.Server.Profiles.FromDto;

public class FirmClientDtoProfile : Profile
{
    public FirmClientDtoProfile()
    {
        CreateMap<FirmClientDto, FirmClient>();
    }
}
