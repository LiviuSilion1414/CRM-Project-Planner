using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Server.Controllers;

[Authorize]
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
	public async Task<ActionResult> AddWorkorder(WorkOrderFormDto entity) { //check inutile se c'è l'attributo [ApiController]-proprietà già controllate
		var workorder = await _repo.SearchWorkorderAsync(entity.Name);
		
		if (workorder == null) {
			await _repo.AddAsync(entity);
			return Ok("Commessa aggiunta con successo!");
		}

		return BadRequest("Commessa già presente!");
	}

    [Authorize]
	[HttpPut("edit")]
	public async Task<ActionResult> EditWorkorder(WorkOrderFormDto entity) { //dto non entity
		var workorder = await _repo.GetForEditAsync(entity.Id);			

		if (workorder == null) { //check inutile - farlo sul dbContext
			return NotFound(NOT_FOUND_RESOURCE); //repo lancia un'eccezione
												//middleware - prende l'eccezione e genera uno codice di stato in base all'eccezione
												//con try/catch
		}

		await _repo.EditAsync(entity);
		return Ok("Commessa modificata con successo!");
	}

    [Authorize]
	[HttpDelete("delete/{id}")]
	public async Task<ActionResult> DeleteWorkorder(int id) {
		var workorder = await _repo.GetForDeleteAsync(id);
		
		if (workorder == null) {
			return NotFound(NOT_FOUND_RESOURCE);
		}

		await _repo.DeleteAsync(id);
		return Ok("Commessa modificata con successo!");
	}

	[Authorize]
	[HttpGet("search/{workorder}")]
	public async Task<List<WorkOrderSelectDto>> SearchWorkorder(string workorder) {
		return await _repo.SearchWorkorderAsync(workorder);
	}

    [Authorize]
	[HttpGet("get/for/edit/{id}")]
	public async Task<WorkOrderFormDto> GetForEdit(int id) {
		return await _repo.GetForEditAsync(id);
	}
	
    [Authorize]
	[HttpGet("get/for/view/{id}")]
	public async Task<WorkOrderViewDto> GetForViewId(int id) {
		return await _repo.GetForViewAsync(id);
	}
	
    [Authorize]
	[HttpGet("get/for/delete/{id}")]
	public async Task<WorkOrderDeleteDto> GetForDeleteId(int id) {
		return await _repo.GetForDeleteAsync(id);
	}

    [Authorize]
	[HttpGet("get/all")]
	public async Task<List<WorkOrderViewDto>> GetAll() {
		return await _repo.GetAllAsync();
	}
}