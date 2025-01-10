namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeSalaryProfile : Profile
{
    public EmployeeSalaryProfile()
    {
        CreateMap<EmployeeSalary, EmployeeSalaryDto>().MaxDepth(1);
        CreateMap<EmployeeSalaryDto, EmployeeSalary>().MaxDepth(1);
    }
}
