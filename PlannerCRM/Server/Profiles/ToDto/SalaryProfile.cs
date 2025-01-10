namespace PlannerCRM.Server.Profiles.ToDto;

public class SalaryProfile : Profile
{
    public SalaryProfile()
    {
        CreateMap<Salary, SalaryDto>().MaxDepth(1);
        CreateMap<SalaryDto, Salary>().MaxDepth(1);
    }
}
