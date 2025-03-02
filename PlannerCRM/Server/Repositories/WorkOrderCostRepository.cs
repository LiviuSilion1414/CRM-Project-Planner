namespace PlannerCRM.Server.Repositories;

public class WorkOrderCostRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(FilterDto filter)
    {
        try
        {
            var model = _mapper.Map<WorkOrderCost>((WorkOrderCostDto)filter.Data);

            await _context.WorkOrderCosts.AddAsync(model);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task EditAsync(FilterDto filter)
    {
        try
        {
            var model = _mapper.Map<WorkOrderCost>((WorkOrderCostDto)filter.Data);

            var existingModel = await _context.WorkOrderCosts.SingleAsync(wo => wo.Guid == model.Guid);
            existingModel = model;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(FilterDto filter)
    {
        try
        {
            var client = await _context.Clients
                                       .Include(c => c.WorkOrders)
                                       .Include(c => c.WorkOrderCosts)
                                       .SingleAsync(c => c.Guid == filter.Id);

            _context.Remove(client);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<WorkOrderCostDto> GetByIdAsync(FilterDto filter)
    {
        try
        {
            var client = await _context.Clients
                                       .Include(c => c.WorkOrders)
                                       .Include(c => c.WorkOrderCosts)
                                       .SingleAsync(c => c.Guid == filter.Id);

            return _mapper.Map<WorkOrderCostDto>(client);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<WorkOrderCostDto>> GetWithPagination(FilterDto filter)
    {
        try
        {
            var clients = await _context.Clients
                                        .OrderBy(c => c.Guid)
                                        .Include(c => c.WorkOrders)
                                        .Include(c => c.WorkOrderCosts)
                                        .ToListAsync();

            return _mapper.Map<List<WorkOrderCostDto>>(clients);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<WorkOrderCostDto>> SearchWorOrderCostByWorkOrderTitle(string worOrderTitle)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrderCosts
                                               .Where(w => EF.Functions.ILike(w.Name, $"%{worOrderTitle}%"))
                                               .Include(w => w.WorkOrder)
                                               .Include(w => w.FirmClient)
                                               .ToListAsync();

            return _mapper.Map<List<WorkOrderCostDto>>(foundWorkOrder);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}