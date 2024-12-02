namespace PlannerCRM.Server.Repositories.Specific;

public class SalaryRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<SalaryDto>> FindAssociatedSalariesByEmployeeId(int employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(s => s.EmployeeSalaries)
            .Include(s => s.Employee)
            .SingleAsync(s => s.EmployeeId == employeeId);

        return _mapper.Map<ICollection<SalaryDto>>(foundSalaries);
    }

    public async Task<ICollection<SalaryDto>> FindAssociatedSalariesByEmployeesIds(params int[] employeesId)
    {
        var foundSalaries = await _context.Salaries
            .Include(s => s.EmployeeSalaries)
            .Include(s => s.Employee)
            .Where(s => employeesId.Any(em => em == s.EmployeeId))
            .ToListAsync();

        return _mapper.Map<ICollection<SalaryDto>>(foundSalaries);
    }
    
    public async Task<SalaryDto> FindLatestSalaryAssignedByEmployeeId(int employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(s => s.EmployeeSalaries)
            .Include(s => s.Employee)
            .OrderBy(s => s.StartDate)
            .FirstAsync(s => s.EmployeeId == employeeId);

        return _mapper.Map<SalaryDto>(foundSalaries);
    }
}