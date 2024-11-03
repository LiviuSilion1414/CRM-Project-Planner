namespace PlannerCRM.Server.Profiles;

public class EmployeeActivityProfile : Profile
{
    public EmployeeActivityProfile()
    {
        CreateMap<EmployeeActivity, EmployeeActivityDto>();
    }
}
