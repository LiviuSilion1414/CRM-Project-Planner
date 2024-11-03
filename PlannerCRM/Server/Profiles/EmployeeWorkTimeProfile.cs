namespace PlannerCRM.Server.Profiles;

public class EmployeeWorkTimeProfile : Profile
{
    public EmployeeWorkTimeProfile()
    {
        CreateMap<EmployeeWorkTime, EmployeeWorkTimeDto>();
    }
}
