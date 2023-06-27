using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using PlannerCRM.Shared.Feedbacks;
using PlannerCRM.Server.Services;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkTimeRecordController : ControllerBase
{
    private readonly WorkTimeRecordRepository _repo;
    private readonly Logger<WorkTimeRecordRepository> _logger;

    public WorkTimeRecordController(
        WorkTimeRecordRepository repo,
        Logger<WorkTimeRecordRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddWorkTimeRecord(WorkTimeRecordFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKTIMERECORD_ADD);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error,nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error,argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error,duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.Log(LogLevel.Error,dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return BadRequest(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }

    [HttpPut("edit")]
    public async Task<ActionResult> EditWorkTimeRecord(WorkTimeRecordFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKTIMERECORD_EDIT);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error,nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error,argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        }
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.Log(LogLevel.Error,keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return NotFound(keyNotFoundExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error,duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.Log(LogLevel.Error,dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return BadRequest(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }

    [HttpGet("get/{workOrderId}/{activityId}/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecord(int workOrderId, int activityId, int employeeId)
    {
        try
        {
            return await _repo.GetAsync(workOrderId, activityId, employeeId);
        }
        catch (KeyNotFoundException exc)
        {
            _logger.Log(LogLevel.Error,exc.Message, exc.StackTrace);
            return null;
        }
    }

    [HttpGet("get/all")]
    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecords()
    {
        try
        {
            return await _repo.GetAllAsync();
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.Message, exc.StackTrace);
            return new List<WorkTimeRecordViewDto>();
        }
    }

    [HttpGet("get/by/employee/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByWorkOrder(int employeeId)
    {
        try
        {
            return await _repo.GetByEmployeeIdAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.Message, exc.StackTrace);
            return new WorkTimeRecordViewDto();
        }
    }
}
