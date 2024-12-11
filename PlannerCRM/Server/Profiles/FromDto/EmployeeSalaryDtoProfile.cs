namespace PlannerCRM.Server.Profiles.FromDto;

public class EmployeeSalaryDtoProfile : Profile
{
    public EmployeeSalaryDtoProfile()
    {
        CreateMap<EmployeeSalaryDto, EmployeeSalary>();
    }
}
