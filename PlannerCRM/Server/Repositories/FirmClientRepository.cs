namespace PlannerCRM.Server.Repositories;

public class FirmClientRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(FirmClientDto dto)
    {
        try
        {
            var model = _mapper.Map<FirmClient>(dto);

            await _context.Clients.AddAsync(model);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {

            throw;
        }
    }

    public async Task Update(FirmClientDto dto)
    {
        try
        {
            var existingModel = await _context.Clients.SingleAsync(cl => cl.Id == dto.id);

            existingModel.Name = dto.name;
            existingModel.VatNumber = dto.vatNumber;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Delete(FirmClientFilterDto filter)
    {
        try
        {
            var client = await _context.Clients
                                       .Include(c => c.WorkOrders)
                                       .SingleAsync(c => c.Id == filter.firmClientId);

            _context.Remove(client);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<FirmClientDto> Get(FirmClientFilterDto filter)
    {
        try
        {
            var client = await _context.Clients
                                       .Include(c => c.WorkOrders)
                                       .SingleAsync(c => c.Id == filter.firmClientId);

            return _mapper.Map<FirmClientDto>(client);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<FirmClientDto>> List(FirmClientFilterDto filter)
    {
        try
        {
            var clients = await _context.Clients
                                        .OrderBy(c => c.Id)
                                        .Include(c => c.WorkOrders)
                                        .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                        .ToListAsync();

            return _mapper.Map<List<FirmClientDto>>(clients);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<FirmClientDto>> Search(FirmClientFilterDto filter)
    {
        try
        {
            var foundClients = await _context.Clients
                                             .Where(cl => EF.Functions.ILike(cl.Name, $"%{filter.searchQuery}%"))
                                             .Include(cl => cl.WorkOrders)
                                             .ToListAsync();

            return _mapper.Map<List<FirmClientDto>>(foundClients);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(FirmClientFilterDto filter)
    {
        try
        {
            var foundWorkOrders = await _context.WorkOrders
                                                .Include(wo => wo.FirmClient)
                                                .Include(wo => wo.Activities)
                                                .Where(wo => wo.FirmClientId == filter.firmClientId)
                                                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrders);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}