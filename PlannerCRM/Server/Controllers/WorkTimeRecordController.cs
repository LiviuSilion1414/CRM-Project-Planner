using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkTimeRecordController : ControllerBase
{
    private readonly WorkTimeRecordRepository _repo;

    public WorkTimeRecordController(WorkTimeRecordRepository repo) {
        _repo = repo;
    }

    [HttpPost("add")]
    public async Task AddWorkTimeRecord(WorkTimeRecordFormDTO entity) {
        await _repo.AddAsync(entity);
    }

    [HttpPut("edit")]
    public async Task EditWorkTimeRecord(WorkTimeRecordFormDTO entity) {
        await _repo.EditAsync(entity);
    }


    [HttpGet("get/{id}")]
    public async Task<WorkTimeRecordViewDTO> GetWorkTimeRecord(int id) {
        return await _repo.GetAsync(id);
    }

    [HttpGet("get/all")]
    public async Task<List<WorkTimeRecordViewDTO>> GetAllWorkTimeRecords() {
        return await _repo.GetAllAsync();
    }

    [HttpGet("get/all/by/workorder/{workorderId}")]
    public async Task<List<WorkTimeRecordViewDTO>> GetAllWorkTimeRecordsByWorkOrder (int workorderId) {
        return await _repo.GetAllAsync(workorderId);
    }
}
