namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeDtoProfile : Profile
{
    public EmployeeDtoProfile()
    {
        CreateMap<Employee, EmployeeDto>().MaxDepth(1);
        CreateMap<EmployeeDto, Employee>().MaxDepth(1);
    }
}
