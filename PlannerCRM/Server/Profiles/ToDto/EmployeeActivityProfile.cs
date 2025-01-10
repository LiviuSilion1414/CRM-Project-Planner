namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeActivityProfile : Profile
{
    public EmployeeActivityProfile()
    {
        CreateMap<EmployeeActivity, EmployeeActivityDto>().MaxDepth(1);
        CreateMap<EmployeeActivityDto, EmployeeActivity>().MaxDepth(1);
    }
}
