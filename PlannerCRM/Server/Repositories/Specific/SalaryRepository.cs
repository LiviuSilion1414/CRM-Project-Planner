namespace PlannerCRM.Server.Repositories.Specific;

public class SalaryRepository(AppDbContext context, IMapper mapper)
    : Repository<Salary, SalaryDto>(context, mapper), IRepository<Salary, SalaryDto>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public override async Task AddAsync(Salary model)
    {
        await _context.EmployeeSalaries.AddAsync(
            new EmployeeSalary
            {
                EmployeeId = model.EmployeeId,
                SalaryId = model.Id
            }
        );

        await _context.SaveChangesAsync();
        
        await base.AddAsync(model);
    }

    public override async Task EditAsync(Salary model, int id)
    {
        var existingSalary = await _context.EmployeeSalaries
            .SingleAsync(ex => ex.EmployeeId == model.Id && ex.SalaryId == id);
        
        existingSalary.EmployeeId = model.EmployeeId;
        existingSalary.SalaryId = model.Id;

        _context.Update(existingSalary);  //it could be redundant

        await _context.SaveChangesAsync();

        await base.EditAsync(model, id);
    }

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