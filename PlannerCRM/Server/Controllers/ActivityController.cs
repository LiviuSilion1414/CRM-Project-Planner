using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.ActivityDto.Views;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ActivityRepository _repo;
    public ActivityController(ActivityRepository repo) {
        _repo = repo;
    }

    [HttpPost("add")]
    public async Task AddActivity(ActivityForm entity) {
        await _repo.AddAsync(entity);
    }

    [HttpPut("edit")]
    public async Task EditActivity(ActivityForm entity) {
        await _repo.EditAsync(entity);
    }    

    [HttpGet("get/{id}")]
    public async Task<ActivityViewDTO> GetById(int id) {
        return await _repo.GetAsync(id);
    }

    [HttpGet("get/all")]
    public async Task<List<ActivityViewDTO>> GetAll() {
        return await _repo.GetAllAsync();
    }

    [HttpDelete("delete/{id}")]
    public async Task DeleteActivity(int id) {
        await _repo.DeleteAsync(id);
    }
}