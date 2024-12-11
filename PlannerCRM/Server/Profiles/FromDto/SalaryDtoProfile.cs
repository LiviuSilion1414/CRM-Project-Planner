namespace PlannerCRM.Server.Profiles.FromDto;

public class SalaryDtoProfile : Profile
{
    public SalaryDtoProfile()
    {
        CreateMap<SalaryDto, Salary>();
    }
}
