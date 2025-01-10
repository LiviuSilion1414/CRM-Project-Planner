namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeWorkTimeProfile : Profile
{
    public EmployeeWorkTimeProfile()
    {
        CreateMap<EmployeeWorkTime, EmployeeWorkTimeDto>().MaxDepth(1);
        CreateMap<EmployeeWorkTimeDto, EmployeeWorkTime>().MaxDepth(1);
    }
}
