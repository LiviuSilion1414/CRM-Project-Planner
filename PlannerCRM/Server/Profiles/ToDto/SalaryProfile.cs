namespace PlannerCRM.Server.Profiles.ToDto;

public class SalaryProfile : Profile
{
    public SalaryProfile()
    {
        CreateMap<Salary, SalaryDto>();
    }
}
