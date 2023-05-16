using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Server.CustomExceptions;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Form;
using PlannerCRM.Shared.DTOs.WorkTimeDto.Views;
using static PlannerCRM.Shared.Constants.ConstantValues;

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
    public async Task<ActionResult> AddWorkTimeRecord(WorkTimeRecordFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok("Orario di lavoro aggiunto con successo!");
        } catch (NullReferenceException nullRefExc) {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);           
        } catch (ArgumentNullException argNullExc) {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        } catch (DuplicateElementException duplicateElemExc) {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        } catch (DbUpdateException dbUpdateExc) {
            _logger.LogError(dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return BadRequest(dbUpdateExc.Message);
        } catch (Exception exc) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }

    [HttpPut("edit")]
    public async Task<ActionResult> EditWorkTimeRecord(WorkTimeRecordFormDto dto) {
        try {
            await _repo.EditAsync(dto);

            return Ok("Orario di lavoro modificato con successo!");
        } catch (NullReferenceException nullRefExc) {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);           
        } catch (ArgumentNullException argNullExc) {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        } catch (KeyNotFoundException keyNotFoundExc) {
            _logger.LogError(keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return NotFound(keyNotFoundExc.Message);
        } catch (DuplicateElementException duplicateElemExc) {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        } catch (DbUpdateException dbUpdateExc) {
            _logger.LogError(dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return BadRequest(dbUpdateExc.Message);
        } catch (Exception exc) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }

    [HttpGet("get/{workTimeRecordId}")]
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecord(int workTimeRecordId) {
        try {
            return await _repo.GetAsync(workTimeRecordId);

        } catch (Exception exc) {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new WorkTimeRecordViewDto();
        }
    }

    [HttpGet("get/all")]
    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecords() {
        try {
            return await _repo.GetAllAsync();
        } catch (Exception exc) {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new List<WorkTimeRecordViewDto>();
        }
    }

    [HttpGet("get/all/by/employee/{employeeId}")]
    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecordsByWorkOrder(int employeeId) {
        try {
            return await _repo.GetAllAsync(employeeId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new List<WorkTimeRecordViewDto>();
        }
    }

    [HttpGet("get/size/by/employee/{employeeId}")]
    public async Task<int> GetWorkTimeRecordsSize(int employeeId) {
        try  {
            return await _repo.GetWorkTimeRecordsSizeByEmployeeId(employeeId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message, exc.StackTrace);
            return INVALID_ID;
        }
    }
}
