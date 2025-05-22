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

            model.FkIdFirmClientNavigation = await _context.FirmClients.FindAsync(dto.fkIdFirmClient);
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
            var workOrder = await _context.WorkOrders
                                        .Include(w => w.Activities)
                                        .SingleAsync(w => w.Id == dto.id);

            workOrder = _mapper.Map<WorkOrder>(dto);

            await _context.SaveChangesAsync();
        } catch
        {
            throw;
        }
    }

    public async Task Delete(WorkOrderFilterDto filter)
    {
        try
        {
            var workOrder = await _context.WorkOrders
                .Include(w => w.Activities)
                .SingleAsync(w => w.Id == filter.workOrderId);

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
                                          .SingleAsync(w => w.Id == filter.id);

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

    public async Task<List<WorkOrderDto>> Search(WorkOrderFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .Where(wo => EF.Functions.Like(wo.Name, $"%{filter.searchQuery}%"))
                                               .Include(wo => wo.FkIdFirmClientNavigation)
                                               .Include(wo => wo.Activities)
                                               .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        } catch
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByWorkOrderId(WorkOrderFilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                                                .AsNoTracking()
                                                .AsSplitQuery()
                                                .Include(ac => ac.FkIdWorkOrderNavigation)
                                                .Include(ac => ac.EmployeeActivities)
                                                .Where(ac => ac.FkIdWorkOrder == filter.id)
                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        } catch
        {
            throw;
        }
    }

    public async Task<List<WorkOrderDto>> FindAssociatedWorkOrdersByClientId(WorkOrderFilterDto filter)
    {
        try
        {
            var foundWorkOrder = await _context.WorkOrders
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .Include(wo => wo.FkIdFirmClientNavigation)
                                               .Include(wo => wo.Activities)
                                               .Where(wo => wo.FkIdFirmClient == filter.id)
                                               .ToListAsync();

            return _mapper.Map<List<WorkOrderDto>>(foundWorkOrder);
        } catch
        {
            throw;
        }
    }
}