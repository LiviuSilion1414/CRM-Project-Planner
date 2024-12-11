namespace PlannerCRM.Server.Profiles.FromDto;

public class EmployeeActivityDtoProfile : Profile
{
    public EmployeeActivityDtoProfile()
    {
        CreateMap<EmployeeActivityDto, EmployeeActivity>();
    }
}
