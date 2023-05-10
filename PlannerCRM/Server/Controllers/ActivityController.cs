using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.Models;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ActivityRepository _repo;
    
    public ActivityController(ActivityRepository repo) {
        _repo = repo;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<ActionResult> AddActivity(ActivityFormDto entity) {
        if (!ModelState.IsValid) {
            return BadRequest("Input non valido!");
        }

        var activities = await _repo.GetForViewAsync(entity.Id);
        
        if (activities == null) {
            await _repo.AddAsync(entity);
            return Ok("Attività aggiunta con successo!");
        }

        return BadRequest("Attività già presente su questa commessa!");
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<ActionResult> EditActivity(ActivityFormDto entity) {
        if (!ModelState.IsValid) {
            return BadRequest("Input non valido!");
        }

        var activities = await _repo.GetActivitiesPerWorkOrderAsync(entity.WorkOrderId ?? throw new NullReferenceException());
        
        if (activities != null || activities.Count() != 0) {
            await _repo.EditAsync(entity);
            return Ok("Attività modificata con successo!");
        }

        return NotFound(NOT_FOUND_RESOURCE);
    }    

    [Authorize]
    [HttpGet("get/{id}")]
    public async Task<ActivityViewDto> GetForView(int id) {
        return await _repo.GetForViewAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/edit/{id}")]
    public async Task<ActivityFormDto> GetForEdit(int id) {
        return await _repo.GetForEditAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/delete/{id}")]
    public async Task<ActivityDeleteDto> GetForDelete(int id) {
        return await _repo.GetForDeleteAsync(id);
    }

    [Authorize]
    [HttpGet("get/activity/per/workorder/{workorderId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkorderAsync(int workorderId) {
        return await _repo.GetActivitiesPerWorkOrderAsync(workorderId);
    }

    [Authorize]
    [HttpGet("get/activity/per/employee/{employeeId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerEmployee(int employeeId) {
        return await _repo.GetActivityByJuniorEmployeeId(employeeId);
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<ActivityViewDto>> GetAll() {
        return await _repo.GetAllAsync();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteActivity(int id) {
        var activity = await _repo.GetForViewAsync(id);

        if (activity == null) {
            return NotFound(NOT_FOUND_RESOURCE);
        } 

        await _repo.DeleteAsync(id);
        return Ok("Attività eliminata con successo");
    }
}