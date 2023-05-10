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
    public async Task<ActionResult> AddActivity(ActivityFormDto activityFormDto) {
        var activities = await _repo.GetForViewAsync(activityFormDto.Id);
        
        if (activities == null) {
            await _repo.AddAsync(activityFormDto);

            return Ok("Attività aggiunta con successo!");
        }

        return BadRequest("Attività già presente su questa commessa!");
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<ActionResult> EditActivity(ActivityFormDto activityFormDto) {
        var activities = await _repo.GetActivitiesPerWorkOrderAsync(activityFormDto.WorkOrderId ?? throw new NullReferenceException());
        
        if (activities != null || activities.Count() != 0) {
            await _repo.EditAsync(activityFormDto);

            return Ok("Attività modificata con successo!");
        }

        return NotFound(NOT_FOUND_RESOURCE);
    }    

    [Authorize]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetForView(int activityId) {
        return await _repo.GetForViewAsync(activityId);
    }

    [Authorize]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetForEdit(int activityId) {
        return await _repo.GetForEditAsync(activityId);
    }

    [Authorize]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetForDelete(int activityId) {
        return await _repo.GetForDeleteAsync(activityId);
    }

    [Authorize]
    [HttpGet("get/activity/per/workorder/{workOrderId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkorderAsync(int workOrderId) {
        return await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);
    }

    [Authorize]
    [HttpGet("get/activity/per/employee/{employeeId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerEmployee(int employeeId) {
        return await _repo.GetActivityByEmployeeId(employeeId);
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<ActivityViewDto>> GetAll() {
        return await _repo.GetAllAsync();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<ActionResult> DeleteActivity(int activityId) {
        var activity = await _repo.GetForViewAsync(activityId);

        if (activity == null) {
            return NotFound(NOT_FOUND_RESOURCE);
        } 

        await _repo.DeleteAsync(activityId);
        
        return Ok("Attività eliminata con successo");
    }
}