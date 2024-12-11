namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeActivityProfile : Profile
{
    public EmployeeActivityProfile()
    {
        CreateMap<EmployeeActivity, EmployeeActivityDto>();
    }
}
