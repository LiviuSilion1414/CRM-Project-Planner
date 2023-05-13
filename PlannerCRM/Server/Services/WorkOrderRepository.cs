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

	public async Task AddAsync(WorkOrderFormDto workOrderFormDto) {
		_db.WorkOrders.Add(new WorkOrder {
			Name = workOrderFormDto.Name,
			StartDate = workOrderFormDto.StartDate ?? throw new NullReferenceException(),
			FinishDate = workOrderFormDto.FinishDate ?? throw new NullReferenceException()
		});

		await _db.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id) {
		var workOrderDelete = await _db.WorkOrders.SingleOrDefaultAsync(w => w.Id == id);
		
		if (workOrderDelete == null) {
			return;
		}
		_db.WorkOrders.Remove(workOrderDelete);
		
		await _db.SaveChangesAsync();
	}

	public async Task<bool> EditAsync(WorkOrderFormDto workOrderFormDto) {
		var model = await _db.WorkOrders.SingleOrDefaultAsync(w => w.Id == workOrderFormDto.Id);
		
		if (model == null) {
			return false;
		}

		model.Id = workOrderFormDto.Id;
		model.Name = workOrderFormDto.Name;
		model.StartDate = workOrderFormDto.StartDate ?? throw new NullReferenceException();
		model.FinishDate = workOrderFormDto.FinishDate ?? throw new NullReferenceException();

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

