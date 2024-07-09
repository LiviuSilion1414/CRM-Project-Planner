namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository(
    AppDbContext dbContext,
    DtoValidatorUtillity validator) : IRepository<WorkOrderFormDto>, IWorkOrderRepository
{
	private readonly AppDbContext _dbContext = dbContext;
	private readonly DtoValidatorUtillity _validator = validator;

    public async Task AddAsync(WorkOrderFormDto dto) {
		var isValid = await _validator.ValidateWorkOrderAsync(dto, OperationType.ADD);
		
		if (isValid) {
			await _dbContext.WorkOrders.AddAsync(dto.MapToWorkOrder());

			await _dbContext.SaveChangesAsync();

			var workOrder = await _dbContext.WorkOrders
				.SingleAsync(wo => EF.Functions.ILike(wo.Name, dto.Name) && wo.ClientId == dto.ClientId);

			await SetForeignKeyToClientAsync(workOrder, OperationType.ADD);
		}
	}

	private async Task SetForeignKeyToClientAsync(WorkOrder workOrder, OperationType operationType) {
		if (!string.IsNullOrEmpty(workOrder.Name) && await _dbContext.WorkOrders.AnyAsync(wo => wo.Id == workOrder.Id)) {
			if (operationType == OperationType.ADD) {
				await _dbContext.ClientWorkOrders.AddAsync(workOrder.MapToClientWorkOrder());
			} else {
				var clientWorkOrder = await _dbContext.ClientWorkOrders
					.SingleAsync(clwo => clwo.WorkOrderId == workOrder.Id);

				clientWorkOrder.ClientId = workOrder.ClientId;
			}

			await _dbContext.SaveChangesAsync();
		}
	}

	public async Task DeleteAsync(int id) {
		var workOrderDelete = await _validator.ValidateDeleteWorkOrderAsync(id);

		if (workOrderDelete is not null) {

			workOrderDelete.IsDeleted = true;
			
			await _dbContext.SaveChangesAsync();
		}
	}

	public async Task EditAsync(WorkOrderFormDto dto) {
		var isValid = await _validator.ValidateWorkOrderAsync(dto, OperationType.EDIT);

		if (isValid) {
			var model = await _dbContext.WorkOrders
				.SingleAsync(wo => !wo.IsDeleted && !wo.IsCompleted && wo.Id == dto.Id);
			
			model = dto.MapToWorkOrder();
			model.Client = await GetClientByWorkOrderId(dto.ClientId);

			await _dbContext.SaveChangesAsync();
			
			await SetForeignKeyToClientAsync(model, OperationType.EDIT);
		}
	}

	public async Task<FirmClient> GetClientByWorkOrderId(int clientId)
	{
		return await _dbContext.Clients
			.SingleAsync(cl => cl.Id == clientId);
    }
	
	public async Task<WorkOrderDeleteDto> GetForDeleteByIdAsync(int id) {
		return await _dbContext.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => wo.MapToWorkOrderDeleteDto())
			.SingleAsync(wo => wo.Id == id);
	}

	public async Task<WorkOrderViewDto> GetForViewByIdAsync(int id) {
		return await _dbContext.WorkOrders
			.Select(wo => wo.MapToWorkOrderViewDto())
			.SingleAsync(wo => wo.Id == id);
	}
	
	public async Task<WorkOrderFormDto> GetForEditByIdAsync(int id) {
		var workOrder = await _dbContext.WorkOrders
			.Where(wo => !wo.IsDeleted || !wo.IsCompleted)
			.Select(wo => wo.MapToWorkOrderFormDto())
			.SingleAsync(wo => wo.Id == id);
		workOrder.ClientName = (await GetClientByWorkOrderId(workOrder.ClientId)).Name;

		return workOrder;
	}

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrderName) {
		return await _dbContext.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted &&
				EF.Functions.ILike(wo.Name, $"%{workOrderName}%"))
			.Select(wo => wo.MapToWorkOrderSelectDto())
			.ToListAsync();
    }

	public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5) {
		return await _dbContext.WorkOrders
			.OrderBy(wo => wo.Name)
			.Skip(limit)
			.Take(offset)
            .AsSplitQuery()
			.Select(wo => wo.MapToWorkOrderViewDto())
			.ToListAsync();
	}

	public async Task<int> GetWorkOrdersSizeAsync() => await _dbContext.WorkOrders.CountAsync();
}