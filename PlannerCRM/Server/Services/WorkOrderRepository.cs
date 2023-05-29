using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Shared.CustomExceptions;
using static PlannerCRM.Shared.Constants.ExceptionsMessages;

namespace PlannerCRM.Server.Services;

public class WorkOrderRepository
{
	private readonly AppDbContext _db;

	public WorkOrderRepository(AppDbContext db) {
		_db = db;
	}

	public async Task AddAsync(WorkOrderFormDto dto) {
		if (dto.GetType() == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var isAlreadyPresent = await _db.WorkOrders
            .SingleOrDefaultAsync(em => em.Id == dto.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException(OBJECT_ALREADY_PRESENT);
        }

		await _db.WorkOrders.AddAsync(new WorkOrder {
			Name = dto.Name,
			StartDate = dto.StartDate ?? throw new NullReferenceException(NULL_PROP),
			FinishDate = dto.FinishDate ?? throw new NullReferenceException(NULL_PROP)
		});

		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_GOING_FORWARD);
        }
	}

	public async Task DeleteAsync(int id) {
		var workOrderDelete = await _db.WorkOrders
			.SingleOrDefaultAsync(w => w.Id == id);
		
		if (workOrderDelete == null) {
			throw new KeyNotFoundException(IMPOSSIBLE_DELETE);
		}
		_db.WorkOrders.Remove(workOrderDelete);
		
		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
        }
	}

	public async Task EditAsync(WorkOrderFormDto dto) {
		if (dto == null) {
            throw new NullReferenceException(NULL_OBJECT);
        }
        var HasPropertiesNull = dto.GetType().GetProperties()
            .Any(prop => prop.GetValue(dto) == null);
        if (HasPropertiesNull) {
            throw new ArgumentNullException(NULL_PARAM);
        }
        
        var model = await _db.WorkOrders
			.SingleOrDefaultAsync(wo => wo.Id == dto.Id);

        if (model == null) {
            throw new KeyNotFoundException(OBJECT_NOT_FOUND);
        }

		model.Id = dto.Id;
		model.Name = dto.Name;
		model.StartDate = dto.StartDate ?? throw new NullReferenceException(NULL_PROP);
		model.FinishDate = dto.FinishDate ?? throw new NullReferenceException(NULL_PROP);

		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException(IMPOSSIBLE_SAVE_CHANGES);
        }
	}
	
	public async Task<WorkOrderDeleteDto> GetForDeleteAsync(int id) {
		return await _db.WorkOrders
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
				FinishDate = wo.FinishDate})
			.SingleOrDefaultAsync(wo => wo.Id == id);
	}
	
	public async Task<WorkOrderFormDto> GetForEditAsync(int id) {
		return await _db.WorkOrders
			.Select(wo => new WorkOrderFormDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate})
			.SingleOrDefaultAsync(wo => wo.Id == id);
	}
	
	public async Task<List<WorkOrderViewDto>> GetAllAsync() {
		return await _db.WorkOrders
			.Select(wo => new WorkOrderViewDto {
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate})
			.ToListAsync();
	}

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder) {
        return await _db.WorkOrders
			.Select(wo => new WorkOrderSelectDto{
				Id = wo.Id,
				Name = wo.Name})
			.Where(e => EF.Functions.Like(e.Name , $"%{workOrder}%"))
			.ToListAsync();
    }
}

