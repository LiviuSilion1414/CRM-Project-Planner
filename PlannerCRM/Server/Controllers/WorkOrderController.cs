using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkOrderController : ControllerBase
{
	private readonly WorkOrderRepository _repo;

	public WorkOrderController(WorkOrderRepository repo) {
		_repo = repo;
	}

    [Authorize]
	[HttpPost("add")]
	public async Task AddWorkorder(WorkorderForm model) {
		await _repo.AddAsync(model);
	}

    [Authorize]
	[HttpPut("edit")]
	public async Task EditWorkorder(WorkorderForm model) {
		await _repo.EditAsync(model);
	}

    [Authorize]
	[HttpDelete("delete/{id}")]
	public async Task DeleteWorkorder(int id) {
		await _repo.DeleteAsync(id);
	}

	[Authorize]
	[HttpGet("search/{workorder}")]
	public async Task<List<WorkorderSelectDTO>> SearchWorkorder(string workorder) {
		return await _repo.SearchWorkorderAsync(workorder);
	}

    [Authorize]
	[HttpGet("get/for/edit/{id}")]
	public async Task<WorkorderForm> GetForEdit(int id) {
		return await _repo.GetForEditAsync(id);
	}
	
    [Authorize]
	[HttpGet("get/for/view/{id}")]
	public async Task<WorkorderViewDTO> GetForViewId(int id) {
		return await _repo.GetForViewAsync(id);
	}
	
    [Authorize]
	[HttpGet("get/for/delete/{id}")]
	public async Task<WorkorderDeleteDTO> GetForDeleteId(int id) {
		return await _repo.GetForDeleteAsync(id);
	}

    [Authorize]
	[HttpGet("get/all")]
	public async Task<List<WorkorderViewDTO>> GetAll() {
		return await _repo.GetAllAsync();
	}
}