namespace PlannerCRM.Server.Repositories;

public class FirmClientRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<FirmClient>((FirmClientDto)filter.Data);

            await _context.Clients.AddAsync(model);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {

            throw;
        }
    }

    public async Task EditAsync(SearchFilterDto filter)
    {
        try
        {
            var model = _mapper.Map<FirmClient>((FirmClientDto)filter.Data);

            var existingModel = await _context.Clients.SingleAsync(cl => cl.Guid == model.Guid);
            existingModel = model;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(SearchFilterDto filter)
    {
        try
        {
            var client = await _context.Clients
                                       .Include(c => c.WorkOrders)
                                       .Include(c => c.WorkOrderCosts)
                                       .SingleAsync(c => c.Guid == filter.Guid);

            _context.Remove(client);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<FirmClientDto> GetByIdAsync(SearchFilterDto filter)
    {
        try
        {
            var client = await _context.Clients
                                       .Include(c => c.WorkOrders)
                                       .Include(c => c.WorkOrderCosts)
                                       .SingleAsync(c => c.Guid == filter.Guid);

            return _mapper.Map<FirmClientDto>(client);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<FirmClientDto>> GetWithPagination(SearchFilterDto filter)
    {
        try
        {
            var clients = await _context.Clients
                                        .OrderBy(c => c.Guid)
                                        .Skip(filter.Offset)
                                        .Take(filter.Limit)
                                        .Include(c => c.WorkOrders)
                                        .Include(c => c.WorkOrderCosts)
                                        .ToListAsync();

            return _mapper.Map<List<FirmClientDto>>(clients);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<FirmClientDto>> SearchClientByName(SearchFilterDto filter)
    {
        try
        {
            var foundClients = await _context.Clients
                                             .Where(cl => EF.Functions.ILike(cl.Name, $"%{filter.SearchQuery}%"))
                                             .Include(cl => cl.WorkOrders)
                                             .ToListAsync();

            return _mapper.Map<List<FirmClientDto>>(foundClients);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(Guid clientId)
    {
        try
        {
            var foundWorkOrders = await _context.WorkOrders
                                                .Include(wo => wo.FirmClient)
                                                .Include(wo => wo.Activities)
                                                .Include(wo => wo.WorkOrderCost)
                                                .Where(wo => wo.FirmClientId == clientId)
                                                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrders);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}