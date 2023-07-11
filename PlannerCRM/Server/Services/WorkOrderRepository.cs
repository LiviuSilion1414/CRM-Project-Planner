using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Server.DataAccess;
using PlannerCRM.Shared.Constants;
using PlannerCRM.Server.Models;

namespace PlannerCRM.Server.Services;

public class WorkOrderRepository
{
	private readonly AppDbContext _db;

	public WorkOrderRepository(AppDbContext db) {
		_db = db;
	}

	public async Task AddAsync(WorkOrderFormDto dto) {
		if (dto.GetType() is null) 
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);

        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) is null);
        
        if (HasPropertiesNull) 
            throw new ArgumentNullException(ExceptionsMessages.NULL_PARAM);
        
        var isAlreadyPresent = await _db.WorkOrders
			.Where(wo => !wo.IsCompleted)
            .SingleOrDefaultAsync(em => em.Id == dto.Id);
        if (isAlreadyPresent != null) 
            throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);

		await _db.WorkOrders.AddAsync(new WorkOrder {
			Id = dto.Id,
			Name = dto.Name,
			StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
			FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP),
			IsDeleted = false,
			IsCompleted = false
		});

		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_GOING_FORWARD);
	}

	public async Task DeleteAsync(int id) {
		var workOrderDelete = await _db.WorkOrders
			.SingleOrDefaultAsync(w => w.Id == id);
		
		if (workOrderDelete is null)
			throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
	
		var hasRelationships = await _db.EmployeeActivity
			.AnyAsync(ea  => ea.Activity.WorkOrderId == workOrderDelete.Id);

			workOrderDelete.IsDeleted = true;
		
		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
	}

	public async Task EditAsync(WorkOrderFormDto dto) {
		if (dto is null)
            throw new NullReferenceException(ExceptionsMessages.NULL_OBJECT);
        
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) is null);
        if (HasPropertiesNull)
            throw new ArgumentNullException(ExceptionsMessages.NULL_PARAM);
        
        var model = await _db.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.SingleOrDefaultAsync(wo => wo.Id == dto.Id);

        if (model is null)
            throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);

		model.Id = dto.Id;
		model.Name = dto.Name;
		model.StartDate = dto.StartDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
		model.FinishDate = dto.FinishDate ?? throw new NullReferenceException(ExceptionsMessages.NULL_PROP);

		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0)
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
	}
	
	public async Task<WorkOrderDeleteDto> GetForDeleteAsync(int id) {
		return await _db.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => new WorkOrderDeleteDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate})
			.SingleOrDefaultAsync(wo => wo.Id == id);
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
			.SingleOrDefaultAsync(wo => wo.Id == id);
	}
	
	public async Task<WorkOrderFormDto> GetForEditAsync(int id) {
		return await _db.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => new WorkOrderFormDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate})
			.SingleOrDefaultAsync(wo => wo.Id == id);
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

	public async Task<int> GetSize() {
		return await _db.WorkOrders.CountAsync();
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
}