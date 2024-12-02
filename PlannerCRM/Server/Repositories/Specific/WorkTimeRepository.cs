namespace PlannerCRM.Server.Repositories.Specific;

public class WorkTimeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Employee> SearchWorkTimeByEmployeeName(string employeeName)
    {
        var foundWorkTime = await _context.Employees
            .Include(em => em.Activities)
            .Include(em => em.Salaries)
            .Include(em => em.WorkTimes)
            .SingleOrDefaultAsync(em => EF.Functions.ILike(em.Name, $"%{employeeName}%"));

        return _mapper.Map<Employee>(foundWorkTime);
    }
}