using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Shared.Feedbacks;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
[ApiController]
[Route("[controller]")]
public class ApplicationUserController : ControllerBase
{
    private readonly ApplicationUserRepository _repo;
    private readonly Logger<ApplicationUserRepository> _logger;

    public ApplicationUserController(
        ApplicationUserRepository repo,
        Logger<ApplicationUserRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add/user")]
    public async Task<ActionResult> AddUser(EmployeeAddFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_ADD);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error,nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.Log(LogLevel.Error,argNullExc.StackTrace, argNullExc.StackTrace);
            return BadRequest(argNullExc);
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.Log(LogLevel.Error,invalidOpExc.Message, invalidOpExc.StackTrace);
            return BadRequest(invalidOpExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.Log(LogLevel.Error,duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return BadRequest(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc) {
            _logger.Log(LogLevel.Error,dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return BadRequest(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return BadRequest(exc.Message);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPut("edit/user")]
    public async Task<ActionResult> EditUser(EmployeeEditFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_EDIT);
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
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.Log(LogLevel.Error,keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return NotFound(keyNotFoundExc.Message);
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.Log(LogLevel.Error,invalidOpExc.Message, invalidOpExc.StackTrace);
            return BadRequest(invalidOpExc.Message);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/user/{email}")]
    public async Task<ActionResult> DeleteUser(string email)
    {
        try
        {
            await _repo.DeleteAsync(email);  

            return Ok(SuccessfulCrudFeedBack.USER_DELETE);            
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.Log(LogLevel.Error,nullRefExc.Message, nullRefExc.StackTrace);
            return BadRequest(nullRefExc.Message);
        }
        catch (KeyNotFoundException keyNotFoundExc)
        {
            _logger.Log(LogLevel.Error,keyNotFoundExc.Message, keyNotFoundExc.StackTrace);
            return BadRequest(keyNotFoundExc.Message);
        }
        catch (Exception exc)
        {
            _logger.Log(LogLevel.Error,exc.StackTrace, exc.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}