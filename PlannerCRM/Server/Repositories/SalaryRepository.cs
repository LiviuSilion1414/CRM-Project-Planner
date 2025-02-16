namespace PlannerCRM.Server.Repositories;

public class SalaryRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(SalaryDto dto)
    {
        var model = _mapper.Map<Salary>(dto);

        _context.Attach(model.Employee);

        await _context.Salaries.AddAsync(model);
        await _context.EmployeeSalaries.AddAsync(new() { EmployeeId = model.EmployeeId, SalaryId = model.Guid });

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(Salary dto)
    {
        var model = _mapper.Map<Salary>(dto);

        _context.Attach(model.Employee);

        var existingSalary = await _context.Salaries.SingleAsync(sl => sl.Guid == model.Guid);

        _context.Update(existingSalary);

        await _context.SaveChangesAsync();
    }

    public async Task<List<SalaryDto>> FindAssociatedSalariesByEmployeeId(Guid employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(s => s.EmployeeSalaries)
            .Include(s => s.Employee)
            .SingleAsync(s => s.EmployeeId == employeeId);

        return _mapper.Map<List<SalaryDto>>(foundSalaries);
    }

    public async Task<List<SalaryDto>> FindAssociatedSalariesByEmployeesIds(params Guid[] employeesId)
    {
        var foundSalaries = await _context.Salaries
            .Include(s => s.EmployeeSalaries)
            .Include(s => s.Employee)
            .Where(s => employeesId.Any(em => em == s.EmployeeId))
            .ToListAsync();

        return _mapper.Map<List<SalaryDto>>(foundSalaries);
    }

    public async Task<SalaryDto> FindLatestSalaryAssignedByEmployeeId(Guid employeeId)
    {
        var foundSalaries = await _context.Salaries
            .Include(s => s.EmployeeSalaries)
            .Include(s => s.Employee)
            .OrderBy(s => s.StartDate)
            .FirstAsync(s => s.EmployeeId == employeeId);

        return _mapper.Map<SalaryDto>(foundSalaries);
    }
}