using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.Models;

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
    public async Task AddActivity(ActivityForm entity) {
        await _repo.AddAsync(entity);
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task EditActivity(ActivityForm entity) {
        await _repo.EditAsync(entity);
    }    

    [Authorize]
    [HttpGet("get/{id}")]
    public async Task<ActivityViewDTO> GetForView(int id) {
        return await _repo.GetForViewAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/edit/{id}")]
    public async Task<ActivityForm> GetForEdit(int id) {
        return await _repo.GetForEditAsync(id);
    }

    [Authorize]
    [HttpGet("get/for/delete/{id}")]
    public async Task<ActivityDeleteDTO> GetForDelete(int id) {
        return await _repo.GetForDeleteAsync(id);
    }

    [Authorize]
    [HttpGet("get/activity/per/workorder/{workorderId}")]
    public async Task<List<ActivityForm>> GetActivitiesPerWorkorderAsync(int workorderId) {
        return await _repo.GetActivitiesPerWorkOrderAsync(workorderId);
    }

    [Authorize]
    [HttpGet("get/activity/per/employee/{employeeId}")]
    public async Task<List<ActivityForm>> GetActivitiesPerEmployee(int employeeId) {
        return await _repo.GetActivityByJuniorEmployeeId(employeeId);
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<ActivityViewDTO>> GetAll() {
        return await _repo.GetAllAsync();
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{id}")]
    public async Task DeleteActivity(int id) {
        await _repo.DeleteAsync(id);
    }
}