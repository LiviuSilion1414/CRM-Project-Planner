using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Server.CustomExceptions;
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
    private readonly Logger<ActivityRepository> _logger;
    
    public ActivityController(
        ActivityRepository repo, 
        Logger<ActivityRepository> logger) 
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<ActionResult> AddActivity(ActivityFormDto dto) {
        try {
            await _repo.AddAsync(dto);

            return Ok("Attività aggiunta con successo!");
        } catch (NullReferenceException nullRefExc) {
            _logger.LogError(nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);
        } catch (ArgumentNullException argNullExc) {
            _logger.Log(LogLevel.Error, argNullExc.Message, "Parametri dell'entità sono null");
            return BadRequest(argNullExc.Message);
        } catch (DuplicateElementException duplicateElemExc) {
            _logger.LogError(duplicateElemExc, duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        } catch (DbUpdateException dbUpdateExc) {
            _logger.LogError(dbUpdateExc, dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }            
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<ActionResult> EditActivity(ActivityFormDto dto) {
        try {
            await _repo.EditAsync(dto);

            return Ok("Attività aggiunta con successo!");
        } catch (NullReferenceException nullRefExc) {
            _logger.LogError(nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);
        } catch (ArgumentNullException argNullExc) {
            _logger.LogError(argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        } catch (KeyNotFoundException keyNotFoundExc) {
            _logger.LogError(keyNotFoundExc, keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return NotFound(keyNotFoundExc.Message);
        } catch (DbUpdateException dbUpdateExc) {
            _logger.LogError(dbUpdateExc, dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }    

    [Authorize]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetForView(int activityId) {
        try {
            return await _repo.GetForViewAsync(activityId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return new ActivityViewDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetForEdit(int activityId) {
        try {
            return await _repo.GetForEditAsync(activityId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return new ActivityFormDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetForDelete(int activityId) {
        try {
            return await _repo.GetForDeleteAsync(activityId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return new ActivityDeleteDto();
        }
    }

    [Authorize]
    [HttpGet("get/activity/per/workorder/{workOrderId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerWorkorderAsync(int workOrderId) {
        try {
            return await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return new List<ActivityFormDto>();
        }
    }

    [Authorize]
    [HttpGet("get/activity/per/employee/{employeeId}")]
    public async Task<List<ActivityFormDto>> GetActivitiesPerEmployee(int employeeId) {
        try {
            return await _repo.GetActivityByEmployeeId(employeeId);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return new List<ActivityFormDto>();
        }
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<ActivityViewDto>> GetAll() {
        try {
            return await _repo.GetAllAsync();
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return new List<ActivityViewDto>();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<ActionResult> DeleteActivity(int activityId) {
        try {
            await _repo.DeleteAsync(activityId);
            
            return Ok("Attività eliminata con successo");
        } catch (InvalidOperationException invalidOpExc) {
            _logger.LogError(invalidOpExc, invalidOpExc.Message, invalidOpExc.StackTrace);
            return BadRequest(invalidOpExc.Message);
        } catch (DbUpdateException dbUpdateExc) {
            _logger.LogError(dbUpdateExc, dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError);
        } catch (Exception exc) {
            _logger.LogError(exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}