namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeWorkTimeProfile : Profile
{
    public EmployeeWorkTimeProfile()
    {
        CreateMap<EmployeeWorkTime, EmployeeWorkTimeDto>();
    }
}
