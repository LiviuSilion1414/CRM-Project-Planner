namespace PlannerCRM.Server.Services;

public class WorkOrderRepository
{
	private readonly AppDbContext _db;
	private readonly DtoValidatorService _validator;
	private readonly Logger<DtoValidatorService> _logger;

	public WorkOrderRepository(AppDbContext db, DtoValidatorService validator, Logger<DtoValidatorService> logger) {
		_db = db;
		_validator = validator;
		_logger = logger;
	}

	public async Task AddAsync(WorkOrderFormDto dto) {
		try	{
			_validator.ValidateWorkOrder(dto, OperationType.ADD, out var isValid);
			
			if (isValid) {
				await _db.WorkOrders.AddAsync(new WorkOrder {
					Id = dto.Id,
					Name = dto.Name,
					StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
					FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
					IsDeleted = false,
					IsCompleted = false
				});

				if (await _db.SaveChangesAsync() == 0) {
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}
			} else {
				throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_ADD);
			}
		} catch (Exception exc) {
			_logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

			throw;
		}
	}

	public async Task DeleteAsync(int id) {
		try {
			var workOrderDelete = await _validator.ValidateDeleteWorkOrderAsync(id);

			workOrderDelete.IsDeleted = true;
			
			if (await _db.SaveChangesAsync() == 0) {
				throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
			}
		} catch (Exception exc) {
			_logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

			throw;
		}
	}

	public async Task EditAsync(WorkOrderFormDto dto) {
        try {
			_validator.ValidateWorkOrder(dto, OperationType.EDIT, out var isValid);

			if (isValid) {
				var model = await _db.WorkOrders
							.SingleAsync(wo => ((!wo.IsDeleted) && (!wo.IsCompleted)) && wo.Id == dto.Id);
				
				model.Id = dto.Id;
				model.Name = dto.Name;
				model.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
				model.FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
		
				if (await _db.SaveChangesAsync() == 0) {
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}
			} else {
				throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
			}
		} catch (Exception exc) {
			_logger.LogError("Error: { } Message: { }", exc.Source, exc.Message);

			throw;
		}
	}
	
	public async Task<WorkOrderDeleteDto> GetForDeleteAsync(int id) {
		return await _db.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => new WorkOrderDeleteDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate})
			.SingleAsync(wo => wo.Id == id);
	}

	public async Task<WorkOrderViewDto> GetForViewAsync(int id) {
		return await _db.WorkOrders
			.Select(wo => new WorkOrderViewDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate,
				IsCompleted = wo.IsCompleted,
				IsDeleted = wo.IsDeleted
			})
			.SingleAsync(wo => wo.Id == id);
	}
	
	public async Task<WorkOrderFormDto> GetForEditAsync(int id) {
		return await _db.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => new WorkOrderFormDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate})
			.SingleAsync(wo => wo.Id == id);
	}

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder) {
        return await _db.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => new WorkOrderSelectDto{
				Id = wo.Id,
				Name = wo.Name})
			.Where(e => EF.Functions.ILike(e.Name , $"%{workOrder}%"))
			.ToListAsync();
    }

	public async Task<List<WorkOrderViewDto>> GetPaginated(int skip = 0, int take = 5) {
		return await _db.WorkOrders
			.OrderBy(wo => wo.Name)
			.Skip(skip)
			.Take(take)
			.Select(wo => new WorkOrderViewDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate,
				IsCompleted = wo.IsCompleted,
				IsDeleted = wo.IsDeleted})
			.ToListAsync();
	}

	public async Task<int> GetSize() => await _db.WorkOrders.CountAsync();
}