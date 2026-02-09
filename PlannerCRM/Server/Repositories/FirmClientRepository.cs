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
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task Update(FirmClientDto dto)
    {
        try
        {
            var existingModel = await _context.FirmClients.FirstAsync(cl => cl.Id == dto.id);

            existingModel.Name = dto.name;
            existingModel.VatNumber = dto.vatNumber;
            existingModel.Email = dto.email;
            existingModel.FiscalCode = dto.fiscalCode;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task Delete(FirmClientDto dto)
    {
        try
        {
            var client = await _context.FirmClients
                                       .AsSplitQuery()
                                       .Include(c => c.WorkOrders)
                                       .FirstAsync(c => c.Id == dto.id);

            _context.Remove(client);

            await _context.SaveChangesAsync();
        } catch (Exception)
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
                                       .FirstAsync(c => c.Id == filter.firmClientId);

            return _mapper.Map<FirmClientDto>(client);
        } catch (Exception)
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
                                            .Where(x => (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim()) &&
                                                        (filter.firmClientId == null || filter.firmClientId == Guid.Empty) || (filter.firmClientId == x.Id)))
                                            .ToListAsync();

            return _mapper.Map<List<FirmClientDto>>(firmClients);
        } catch (Exception)
        {
            throw;
        }
    }
}