namespace PlannerCRM.Server.Repositories.Specific;

public class WorkTimeRepository(AppDbContext context, IMapper mapper)
    : Repository<WorkTime, WorkTimeDto>(context, mapper), IRepository<WorkTime, WorkTimeDto>
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<WorkTimeDto>> SearchWorkTimeByEmployeeName(string employeeName)
    {
        var foundWorkTime = await _context.WorkTimes
            .Include(wt => wt.WorkOrder)
            .Include(wt => wt.ActivityWorkTimes)
            .Include(wt => wt.Activity)
            .Include(wt => wt.Employee)
            .Where(wt => EF.Functions.ILike(wt.Employee.Name, $"%{employeeName}%"))
            .ToListAsync();

        return _mapper.Map<ICollection<WorkTimeDto>>(foundWorkTime);
    }
}