namespace PlannerCRM.Server.Profiles;

public class EmployeeSalaryProfile : Profile
{
    public EmployeeSalaryProfile()
    {
        CreateMap<EmployeeSalary, EmployeeSalaryDto>().MaxDepth(1);
        CreateMap<EmployeeSalaryDto, EmployeeSalary>().MaxDepth(1);
    }
}
