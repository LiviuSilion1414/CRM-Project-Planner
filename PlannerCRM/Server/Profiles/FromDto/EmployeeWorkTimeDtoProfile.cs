namespace PlannerCRM.Server.Profiles.FromDto;

public class EmployeeWorkTimeDtoProfile : Profile
{
    public EmployeeWorkTimeDtoProfile()
    {
        CreateMap<EmployeeWorkTimeDto, EmployeeWorkTime>();
    }
}
