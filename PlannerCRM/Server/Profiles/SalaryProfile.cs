namespace PlannerCRM.Server.Profiles;

public class SalaryProfile : Profile
{
    public SalaryProfile()
    {
        CreateMap<Salary, SalaryDto>();
    }
}
