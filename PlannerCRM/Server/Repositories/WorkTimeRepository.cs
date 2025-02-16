namespace PlannerCRM.Server.Repositories;

public class WorkTimeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(WorkTimeDto dto)
    {
        var model = _mapper.Map<WorkTime>(dto);

        _context.WorkOrders.Attach(model.WorkOrder);
        _context.Activities.Attach(model.Activity);

        await _context.WorkTimes.AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(WorkTimeDto dto)
    {
        var model = _mapper.Map<WorkTime>(dto);

        var existingModel = await _context.WorkTimes.SingleAsync(wt => wt.Guid == model.Guid);
        existingModel = model;

        _context.Update(existingModel);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WorkTimeDto dto)
    {
        var client = await _context.WorkTimes
            .Include(w => w.Activity)
            .Include(w => w.WorkOrder)
            .ThenInclude(w => w.FirmClient)
            .SingleAsync(w => w.Guid == dto.Guid);

        _context.Remove(client);

        await _context.SaveChangesAsync();
    }

    public async Task<WorkTimeDto> GetByIdAsync(Guid id)
    {
        var client = await _context.WorkTimes
            .Include(w => w.Activity)
            .Include(w => w.WorkOrder)
            .ThenInclude(w => w.FirmClient)
            .SingleAsync(w => w.Guid == id);

        return _mapper.Map<WorkTimeDto>(client);
    }

    public async Task<List<WorkTimeDto>> GetWithPagination(int limit, int offset)
    {
        var clients = await _context.WorkTimes
            .OrderBy(w => w.Guid)
            .Skip(offset)
            .Take(limit)
            .Include(w => w.Activity)
            .Include(w => w.WorkOrder)
            .ThenInclude(w => w.FirmClient)
            .ToListAsync();

        return _mapper.Map<List<WorkTimeDto>>(clients);
    }

    public async Task<List<WorkTimeDto>> SearchWorkTimeByEmployeeName(string employeeName)
    {
        var foundWorkTime = await _context.WorkTimes
            .Include(wt => wt.WorkOrder)
            .Include(wt => wt.ActivityWorkTimes)
            .Include(wt => wt.Activity)
            .Include(wt => wt.Employee)
            .Where(wt => EF.Functions.ILike(wt.Employee.Name, $"%{employeeName}%"))
            .ToListAsync();

        return _mapper.Map<List<WorkTimeDto>>(foundWorkTime);
    }
}