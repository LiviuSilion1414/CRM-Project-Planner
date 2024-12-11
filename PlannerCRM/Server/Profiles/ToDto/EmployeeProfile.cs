namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeDtoProfile : Profile
{
    public EmployeeDtoProfile()
    {
        CreateMap<Employee, EmployeeDto>();
    }
}
