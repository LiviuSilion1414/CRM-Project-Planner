namespace PlannerCRM.Server.Repositories;

public class WorkOrderCostRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(WorkOrderCostDto dto)
    {
        var model = _mapper.Map<WorkOrderCost>(dto);

        await _context.WorkOrderCosts.AddAsync(model);

        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(WorkOrderCostDto dto)
    {
        var model = _mapper.Map<WorkOrderCost>(dto);

        var existingModel = await _context.WorkOrderCosts.SingleAsync(wo => wo.Guid == model.Guid);
        existingModel = model;

        _context.Update(existingModel);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WorkOrderCostDto dto)
    {
        var client = await _context.Clients
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .SingleAsync(c => c.Guid == dto.Guid);

        _context.Remove(client);

        await _context.SaveChangesAsync();
    }

    public async Task<WorkOrderCostDto> GetByIdAsync(Guid id)
    {
        var client = await _context.Clients
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .SingleAsync(c => c.Guid == id);

        return _mapper.Map<WorkOrderCostDto>(client);
    }

    public async Task<List<WorkOrderCostDto>> GetWithPagination(int limit, int offset)
    {
        var clients = await _context.Clients
            .OrderBy(c => c.Guid)
            .Skip(offset)
            .Take(limit)
            .Include(c => c.WorkOrders)
            .Include(c => c.WorkOrderCosts)
            .ToListAsync();

        return _mapper.Map<List<WorkOrderCostDto>>(clients);
    }

    public async Task<List<WorkOrderCostDto>> SearchWorOrderCostByWorkOrderTitle(string worOrderTitle)
    {
        var foundWorkOrder = await _context.WorkOrderCosts
            .Where(w => EF.Functions.ILike(w.Name, $"%{worOrderTitle}%"))
            .Include(w => w.WorkOrder)
            .Include(w => w.FirmClient)
            .ToListAsync();

        return _mapper.Map<List<WorkOrderCostDto>>(foundWorkOrder);
    }
}