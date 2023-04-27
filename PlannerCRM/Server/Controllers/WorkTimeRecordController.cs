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


    [HttpGet("get/{activityId}")]
    public async Task<WorkTimeRecordViewDTO> GetWorkTimeRecord(int activityId) {
        return await _repo.GetAsync(activityId);
    }

    [HttpGet("get/all")]
    public async Task<List<WorkTimeRecordViewDTO>> GetAllWorkTimeRecords() {
        return await _repo.GetAllAsync();
    }

    [HttpGet("get/all/by/employee/{employeeId}")]
    public async Task<List<WorkTimeRecordViewDTO>> GetAllWorkTimeRecordsByWorkOrder(int employeeId) {
        return await _repo.GetAllAsync(employeeId);
    }

    [HttpGet("get/size/by/employee/{employeeId}")]
    public async Task<int> GetWorkTimeRecordsSize(int employeeId) {
        return await _repo.GetWorkTimeRecordsSizeByEmployeeId(employeeId);
    }
}
