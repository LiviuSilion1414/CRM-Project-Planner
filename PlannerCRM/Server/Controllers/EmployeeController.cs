using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Views;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static PlannerCRM.Shared.Constants.SuccessfulFeedBack;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repo;
    private readonly Logger<EmployeeRepository> _logger;

    public EmployeeController(
        EmployeeRepository repo,
        Logger<EmployeeRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddUser(EmployeeAddFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(USER_ADD);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc, duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.LogError(dbUpdateExc, dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message);
            return StatusCode(Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<ActionResult> EditUser(EmployeeEditFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(USER_EDIT);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc, nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc, argNullExc.Message, argNullExc.StackTrace);
            return BadRequest(argNullExc.Message);
        }
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.LogError(keyNotFoundExc, keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return NotFound(keyNotFoundExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.LogError(dbUpdateExc, dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return StatusCode(Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message);
            return StatusCode(Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpDelete("delete/{employeeId}")]
    public async Task<ActionResult> DeleteUser(int employeeId)
    {
        try
        {
            await _repo.DeleteAsync(employeeId);

            return Ok(USER_DELETE);
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.LogError(invalidOpExc, invalidOpExc.Message, invalidOpExc.StackTrace);
            return BadRequest(invalidOpExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.LogError(dbUpdateExc, dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return BadRequest(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return StatusCode(Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetForViewById(int employeeId)
    {
        try
        {
            return await _repo.GetForViewAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new EmployeeViewDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeEditFormDto> GetForEditById(int employeeId)
    {
        try
        {
            return await _repo.GetForEditAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new EmployeeEditFormDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetForDeleteById(int employeeId)
    {
        try
        {
            return await _repo.GetForDeleteAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new EmployeeDeleteDto();
        }
    }

    [Authorize]
    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployee(string email)
    {
        try
        {
            return await _repo.SearchEmployeeAsync(email);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new List<EmployeeSelectDto>();
        }
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<EmployeeViewDto>> GetAll()
    {
        try
        {
            return await _repo.GetAllAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new List<EmployeeViewDto>();
        }
    }

    [Authorize]
    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetUserId(string email)
    {
        try
        {
            return await _repo.GetUserIdAsync(email);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new CurrentEmployeeDto();
        }
    }

    [HttpGet("get/id-check/{email}")]
    public async Task<int> GetUserIdCheck(string email)
    {
        try
        {
            var currentEmployee = await _repo.GetUserIdAsync(email);
            return currentEmployee.Id;
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return 0;
        }
    }
}