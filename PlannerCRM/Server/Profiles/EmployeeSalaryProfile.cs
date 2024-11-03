namespace PlannerCRM.Server.Profiles;

public class EmployeeSalaryProfile : Profile
{
    public EmployeeSalaryProfile()
    {
        CreateMap<EmployeeSalary, EmployeeSalaryDto>();
    }
}
