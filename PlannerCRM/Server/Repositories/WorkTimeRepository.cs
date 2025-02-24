namespace PlannerCRM.Server.Repositories;
public class WorkTimeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    public async Task AddAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<WorkTime>(filter);
            _context.WorkOrders.Attach(model.WorkOrder);
            _context.Activities.Attach(model.Activity);
            await _context.WorkTimes.AddAsync(model);
            await _context.SaveChangesAsync();
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task EditAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<WorkTime>(filter);
            var existingModel = await _context.WorkTimes.SingleAsync(wt => wt.Guid == model.Guid);
            _context.Entry(existingModel).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync(SearchFilterDto filter)
    {
        try
        {
            var client = await _context.WorkTimes
                .Include(w => w.Activity)
                .Include(w => w.WorkOrder)
                .ThenInclude(w => w.FirmClient)
                .SingleAsync(w => w.Guid == filter.Guid);
            _context.Remove(client);
            await _context.SaveChangesAsync();
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<WorkTimeDto> GetByIdAsync(SearchFilterDto filter)
    {
        try
        {
            var client = await _context.WorkTimes
                .Include(w => w.Activity)
                .Include(w => w.WorkOrder)
                .ThenInclude(w => w.FirmClient)
                .SingleAsync(w => w.Guid == filter.Guid);
            return _mapper.Map<WorkTimeDto>(client);
        } 
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<WorkTimeDto>> GetWithPagination(SearchFilterDto filter)
    {
        try
        {
            var clients = await _context.WorkTimes
                .OrderBy(w => w.Guid)
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .Include(w => w.Activity)
                .Include(w => w.WorkOrder)
                .ThenInclude(w => w.FirmClient)
                .ToListAsync();
            return _mapper.Map<List<WorkTimeDto>>(clients);
        } 
        catch (Exception ex)
        {
            throw;
        }
    }
}
