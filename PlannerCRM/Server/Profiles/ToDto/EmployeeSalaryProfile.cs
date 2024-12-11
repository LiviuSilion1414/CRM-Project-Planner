namespace PlannerCRM.Server.Profiles.ToDto;

public class EmployeeSalaryProfile : Profile
{
    public EmployeeSalaryProfile()
    {
        CreateMap<EmployeeSalary, EmployeeSalaryDto>();
    }
}
