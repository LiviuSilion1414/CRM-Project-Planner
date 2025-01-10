namespace PlannerCRM.Server.Repositories.Specific;

public class WorkTimeRepository(AppDbContext context, IMapper mapper)
    : Repository<WorkTime, WorkTimeDto>(context, mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<WorkTimeDto> SearchWorkTimeByEmployeeName(string employeeName)
    {
        var foundWorkTime = await _context.WorkTimes
            .Include(wt => wt.WorkOrder)
            .Include(wt => wt.ActivityWorkTimes)
            .Include(wt => wt.Activity)
            .Include(wt => wt.Employee)
            .SingleOrDefaultAsync(wt => EF.Functions.ILike(wt.Employee.Name, $"%{employeeName}%"));

        return _mapper.Map<WorkTimeDto>(foundWorkTime);
    }
}