using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Models;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using PlannerCRM.Server.CustomExceptions;

namespace PlannerCRM.Server.Services;

public class WorkOrderRepository
{
	private readonly AppDbContext _db;

	public WorkOrderRepository(AppDbContext db) {
		_db = db;
	}

	public async Task AddAsync(WorkOrderFormDto workOrderFormDto) {
		if (workOrderFormDto.GetType() == null) {
            throw new NullReferenceException("Oggetto null.");
        }
        var isNull = workOrderFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(workOrderFormDto) != null);
        
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var isAlreadyPresent = await _db.Employees
            .SingleOrDefaultAsync(em => em.Id == workOrderFormDto.Id);
        if (isAlreadyPresent != null) {
            throw new DuplicateElementException("Oggetto già presente");
        }

		await _db.WorkOrders.AddAsync(new WorkOrder {
			Name = workOrderFormDto.Name,
			StartDate = workOrderFormDto.StartDate ?? throw new NullReferenceException(),
			FinishDate = workOrderFormDto.FinishDate ?? throw new NullReferenceException()
		});

		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile proseguire.");
        }
	}

	public async Task DeleteAsync(int id) {
		var workOrderDelete = await _db.WorkOrders
			.SingleOrDefaultAsync(w => w.Id == id);
		
		if (workOrderDelete == null) {
			throw new InvalidOperationException("Impossibile eliminare l'elemento");
		}
		_db.WorkOrders.Remove(workOrderDelete);
		
		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
        }
	}

	public async Task EditAsync(WorkOrderFormDto workOrderFormDto) {
		if (workOrderFormDto == null) {
            throw new NullReferenceException("Oggetto null.");
        }
        var isNull = workOrderFormDto.GetType().GetProperties()
            .All(prop => prop.GetValue(workOrderFormDto) != null);
        if (isNull) {
            throw new ArgumentNullException("Parametri null");
        }
        
        var model = await _db.Activities
			.SingleOrDefaultAsync(wo => wo.Id == workOrderFormDto.Id);

        if (model == null) {
            throw new KeyNotFoundException("Oggetto non trovato");
        }

		model.Id = workOrderFormDto.Id;
		model.Name = workOrderFormDto.Name;
		model.StartDate = workOrderFormDto.StartDate ?? throw new NullReferenceException();
		model.FinishDate = workOrderFormDto.FinishDate ?? throw new NullReferenceException();

		var rowsAffected = await _db.SaveChangesAsync();
        if (rowsAffected == 0) {
            throw new DbUpdateException("Impossibile salvare i dati.");
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

    public async Task<List<WorkOrderSelectDto>> SearchWorkorderAsync(string workOrder) {
        return await _db.WorkOrders
			.Select(wo => new WorkOrderSelectDto{
				Id = wo.Id,
				Name = wo.Name})
			.Where(e => EF.Functions.Like(e.Name , $"%{workOrder}%"))
			.ToListAsync();
    }
}

