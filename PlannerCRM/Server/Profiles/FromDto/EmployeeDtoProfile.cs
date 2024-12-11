namespace PlannerCRM.Server.Profiles.FromDto;

public class EmployeeDtoProfile : Profile
{
    public EmployeeDtoProfile()
    {
        CreateMap<EmployeeDto, Employee>();
    }
}
