using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(WorkOrderDto dto)
    {
        try
        {
            var model = _mapper.Map<WorkOrder>(dto);

            model.CreationDate = DateTime.Now;

            await _context.WorkOrders.AddAsync(model);

            await _context.SaveChangesAsync();
        } catch (Exception ex)
        {
            throw;
        }
    }

    public async Task Update(WorkOrderDto dto)
    {
        try
        {
            var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(x => x.Id == dto.id);

            if (workOrder == null) return;

            workOrder.Name = dto.name;
            workOrder.StartDate = dto.startDate;
            workOrder.EndDate = dto.endDate;

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Delete(WorkOrderDto dto)
    {
        try
        {
            var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(x => x.Id == dto.id);

            if (workOrder == null) return;

            _context.Remove(workOrder);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task<WorkOrderDto> Get(WorkOrderFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .Include(w => w.Activities)
                                          .Include(w => w.FkIdFirmClientNavigation)
                                          .FirstAsync(w => w.Id == filter.id);

            return _mapper.Map<WorkOrderDto>(workOrder);
        } catch
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> List(WorkOrderFilterDto filter)
    {
        try
        {
            var workOrders = await _context.WorkOrders
                                           .AsNoTracking()
                                           .AsSplitQuery()
                                           .OrderBy(w => w.Id)
                                           .Include(w => w.FkIdFirmClientNavigation)
                                           .Include(w => w.Activities)
                                           .Where(x => (filter.firmClientId == null || filter.firmClientId == x.FkIdFirmClient) &&
                                                       (string.IsNullOrEmpty(filter.searchQuery) || x.Name.ToLower().Trim().Contains(filter.searchQuery.ToLower().Trim())))
                                           .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(workOrders);
        } catch
        {
            throw;
        }
    }
}