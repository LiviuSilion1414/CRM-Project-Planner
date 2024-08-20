namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository(
    AppDbContext dbContext,
    DtoValidatorUtillity validator) : IRepository<WorkOrderFormDto>, IWorkOrderRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DtoValidatorUtillity _validator = validator;

    public async Task AddAsync(WorkOrderFormDto dto)
    {
        var isValid = await _validator.ValidateWorkOrderAsync(dto, OperationType.ADD);

        if (isValid)
        {
            await _dbContext.WorkOrders.AddAsync(dto.MapToWorkOrder());

            await _dbContext.SaveChangesAsync();

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => EF.Functions.ILike(wo.Name, dto.Name) && wo.ClientId == dto.ClientId);

            await SetForeignKeyToClientAsync(workOrder, OperationType.ADD);
        }
    }

    private async Task SetForeignKeyToClientAsync(WorkOrder workOrder, OperationType operationType)
    {
        if (!string.IsNullOrEmpty(workOrder.Name) && await _dbContext.WorkOrders.AnyAsync(wo => wo.Id == workOrder.Id))
        {
            if (operationType == OperationType.ADD)
            {
                await _dbContext.ClientWorkOrders.AddAsync(workOrder.MapToClientWorkOrder());
            } else
            {
                var clientWorkOrder = await _dbContext.ClientWorkOrders
                    .SingleAsync(clwo => clwo.WorkOrderId == workOrder.Id);

                clientWorkOrder.ClientId = workOrder.ClientId;
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var workOrderDelete = await _validator.ValidateDeleteWorkOrderAsync(id);

        if (workOrderDelete is not null)
        {

            workOrderDelete.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task EditAsync(WorkOrderFormDto dto)
    {
        var isValid = await _validator.ValidateWorkOrderAsync(dto, OperationType.EDIT);

        if (isValid)
        {
            var model = dto.MapToWorkOrder();
            model.Client = await GetClientByClientIdAsync(dto.ClientId);
            
            _dbContext.Update(model);
            
            await _dbContext.SaveChangesAsync();

            await SetForeignKeyToClientAsync(model, OperationType.EDIT);
        }
    }

    public async Task<FirmClient> GetClientByClientIdAsync(int clientId)
    {
        return await _dbContext.Clients
            .SingleAsync(cl => cl.Id == clientId);
    }

    public async Task<WorkOrderDeleteDto> GetForDeleteByIdAsync(int workOrderId)
    {
        var workOrder = await _dbContext.WorkOrders
            .Where(wo => !wo.IsDeleted && !wo.IsCompleted && wo.Id == workOrderId)
            .SingleAsync();

        workOrder.Client = await GetClientByClientIdAsync(workOrder.ClientId);

        return workOrder.MapToWorkOrderDeleteDto();
    }

    public async Task<WorkOrderViewDto> GetForViewByIdAsync(int workOrderId)
    {
        var workOrder = await _dbContext.WorkOrders
            .Where(wo => wo.Id == workOrderId)
            .SingleAsync();

        workOrder.Client = (await GetClientByClientIdAsync(workOrder.ClientId));

        return workOrder.MapToWorkOrderViewDto();
    }

    public async Task<WorkOrderFormDto> GetForEditByIdAsync(int workOrderId)
    {
        var workOrder = await _dbContext.WorkOrders
            .Where(wo => !wo.IsDeleted || !wo.IsCompleted && wo.Id == workOrderId)
            .Select(wo => wo.MapToWorkOrderFormDto())
            .SingleAsync();
        workOrder.ClientName = (await GetClientByClientIdAsync(workOrder.ClientId)).Name;

        return workOrder;
    }

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrderName)
    {
        return await _dbContext.WorkOrders
            .Where(wo => !wo.IsDeleted || !wo.IsCompleted &&
                EF.Functions.ILike(wo.Name, $"%{workOrderName}%"))
            .Select(wo => wo.MapToWorkOrderSelectDto())
            .ToListAsync();
    }

    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int offset, int limit = 5)
    {
        var workOrders = await _dbContext.WorkOrders
            .OrderBy(wo => wo.Name)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        foreach (var wo in workOrders) 
        { 
            wo.Client = await _dbContext.Clients
                .SingleAsync(cl => cl.Id == wo.ClientId);
        }
        
        return workOrders
            .Select(wo => wo.MapToWorkOrderViewDto())
            .ToList();
    }

    public async Task<int> GetWorkOrdersSizeAsync() => await _dbContext.WorkOrders.CountAsync();
}