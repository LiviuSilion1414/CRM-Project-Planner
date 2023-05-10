using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;

namespace PlannerCRM.Server.Services;

public class WorkOrderRepository
{
	private readonly AppDbContext _db;

	public WorkOrderRepository(AppDbContext db) {
		_db = db;
	}

	public async Task AddAsync(WorkOrderFormDto entity) {
		_db.WorkOrders.Add(new WorkOrder {
			Name = entity.Name,
			StartDate = entity.StartDate ?? throw new NullReferenceException(),
			FinishDate = entity.FinishDate ?? throw new NullReferenceException()
		});

		await _db.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id) {
		var entity = await _db.WorkOrders.SingleOrDefaultAsync(w => w.Id == id);
		
		if (entity == null) {
			return;
		}
		_db.WorkOrders.Remove(entity);
		
		await _db.SaveChangesAsync();
	}

	public async Task<bool> EditAsync(WorkOrderFormDto entity) {
		var model = await _db.WorkOrders.SingleOrDefaultAsync(w => w.Id == entity.Id);
		
		if (model == null) {
			return false;
		}

		model.Id = entity.Id;
		model.Name = entity.Name;
		model.StartDate = entity.StartDate ?? throw new NullReferenceException();
		model.FinishDate = entity.FinishDate ?? throw new NullReferenceException();

		await _db.SaveChangesAsync();
		return true;
	}
	
	public async Task<WorkOrderDeleteDto> GetForDeleteAsync(int id) {
		return await _db.WorkOrders
			.Select(e => new WorkOrderDeleteDto {
				Id = e.Id,
				Name = e.Name,
				StartDate = e.StartDate,
				FinishDate = e.FinishDate})
			.SingleOrDefaultAsync(e => e.Id == id);
	}

	public async Task<WorkOrderViewDto> GetForViewAsync(int id) {
		return await _db.WorkOrders
			.Select(e => new WorkOrderViewDto {
				Id = e.Id,
				Name = e.Name,
				StartDate = e.StartDate,
				FinishDate = e.FinishDate})
			.SingleOrDefaultAsync(e => e.Id == id);
	}
	
	public async Task<WorkOrderFormDto> GetForEditAsync(int id) {
		return await _db.WorkOrders
			.Select(e => new WorkOrderFormDto {
				Id = e.Id,
				Name = e.Name,
				StartDate = e.StartDate,
				FinishDate = e.FinishDate})
			.SingleOrDefaultAsync(e => e.Id == id);
	}
	
	public async Task<List<WorkOrderViewDto>> GetAllAsync() {
		return await _db.WorkOrders
			.Select(e => new WorkOrderViewDto {
				Id = e.Id,
				Name = e.Name,
				StartDate = e.StartDate,
				FinishDate = e.FinishDate})
			.ToListAsync();
	}

    public async Task<List<WorkOrderSelectDto>> SearchWorkorderAsync(string workorder) {
        return await _db.WorkOrders
			.Select(wo => new WorkOrderSelectDto{
				Id = wo.Id,
				Name = wo.Name})
			.Where(e => EF.Functions.Like(e.Name , $"%{workorder}%"))
			.ToListAsync();
    }
}

