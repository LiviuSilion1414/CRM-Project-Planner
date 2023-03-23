using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services.ConcreteClasses;
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

	[HttpPost("add")]
	public async Task AddWorkorder(WorkorderAddFormDTO model) {
		await _repo.AddAsync(model);
	}

	[HttpPut("edit")]
	public async Task EditWorkorder(WorkorderEditFormDTO model) {
		await _repo.EditAsync(model);
	}

	[HttpDelete("delete/{id}")]
	public async Task DeleteWorkorder(int id) {
		await _repo.DeleteAsync(id);
	}

	[HttpGet("get/for/edit/{id}")]
	public async Task<WorkorderEditFormDTO> GetForEdit(int id) {
		return await _repo.GetForEditAsync(id);
	}
	
	[HttpGet("get/for/view/{id}")]
	public async Task<WorkorderViewDTO> GetForViewId(int id) {
		return await _repo.GetForViewAsync(id);
	}
	
	
	[HttpGet("get/for/delete/{id}")]
	public async Task<WorkorderDeleteDTO> GetForDeleteId(int id) {
		return await _repo.GetForDeleteAsync(id);
	}

	[HttpGet("get/all")]
	public async Task<List<WorkorderViewDTO>> GetAll() {
		return await _repo.GetAllAsync();
	}
}