using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class FirmClientRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(FirmClientDto dto)
    {
        try
        {
            var model = _mapper.Map<FirmClient>(dto);
            model.CreationDate = DateTime.Now;

            await _context.FirmClients.AddAsync(model);

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
            var existingModel = await _context.FirmClients.SingleAsync(cl => cl.Id == dto.id);

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
            var client = await _context.FirmClients
                                       .AsSplitQuery()
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
            var client = await _context.FirmClients
                                       .AsNoTracking()
                                       .AsSplitQuery()
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
            var firmClients = await _context.FirmClients
                                            .AsNoTracking()
                                            .AsSplitQuery()
                                            .OrderBy(c => c.Id)
                                            .Include(c => c.WorkOrders).ThenInclude(w => w.Activities)
                                            .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                            .ToListAsync();

            return _mapper.Map<List<FirmClientDto>>(firmClients);
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
            var foundClients = await _context.FirmClients
                                             .AsNoTracking()
                                             .AsSplitQuery()
                                             .Include(cl => cl.WorkOrders)
                                             .Where(cl => EF.Functions.Like(cl.Name, $"%{filter.searchQuery}%"))
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
                                                .AsNoTracking()
                                                .AsSplitQuery()
                                                .Include(wo => wo.FkIdFirmClientNavigation)
                                                .Include(wo => wo.Activities)
                                                .Where(wo => wo.FkIdFirmClient == filter.firmClientId)
                                                .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrders);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}