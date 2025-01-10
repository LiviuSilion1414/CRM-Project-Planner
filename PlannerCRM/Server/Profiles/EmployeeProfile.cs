namespace PlannerCRM.Server.Profiles;

public class EmployeeDtoProfile : Profile
{
    public EmployeeDtoProfile()
    {
        CreateMap<Employee, EmployeeDto>().MaxDepth(1);
        CreateMap<EmployeeDto, Employee>().MaxDepth(1);
    }
}
