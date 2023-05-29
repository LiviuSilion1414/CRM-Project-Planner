using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Shared.CustomExceptions;
using PlannerCRM.Server.Services;
using PlannerCRM.Shared.DTOs.Workorder.Forms;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static PlannerCRM.Shared.Constants.SuccessfulFeedBack;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly WorkOrderRepository _repo;
    private readonly Logger<WorkOrderRepository> _logger;

    public WorkOrderController(
        WorkOrderRepository repo,
        Logger<WorkOrderRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddWorkorder(WorkOrderAddFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(WORKORDER_ADD);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return NotFound(argNullExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return NotFound(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.LogError(dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return NotFound(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return StatusCode(Status503ServiceUnavailable);
        }
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<ActionResult> EditWorkorder(WorkOrderEditFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);
            return Ok(WORKORDER_EDIT);
        }
        catch (NullReferenceException nullRefExc)
        {
            _logger.LogError(nullRefExc.Message, nullRefExc.StackTrace);
            return NotFound(nullRefExc.Message);
        }
        catch (ArgumentNullException argNullExc)
        {
            _logger.LogError(argNullExc.Message, argNullExc.StackTrace);
            return NotFound(argNullExc.Message);
        }
        catch (DuplicateElementException duplicateElemExc)
        {
            _logger.LogError(duplicateElemExc.Message, duplicateElemExc.StackTrace);
            return NotFound(duplicateElemExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.LogError(dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return NotFound(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return NotFound(exc.Message);
        }
    }

    [Authorize]
    [HttpDelete("delete/{workOrderId}")]
    public async Task<ActionResult> DeleteWorkorder(int workOrderId)
    {
        try
        {
            await _repo.DeleteAsync(workOrderId);

            return Ok(WORKORDER_DELETE);
        }
        catch (InvalidOperationException invalidOpExc)
        {
            _logger.LogError(invalidOpExc.Message, invalidOpExc.StackTrace);
            return NotFound(invalidOpExc.Message);
        }
        catch (DbUpdateException dbUpdateExc)
        {
            _logger.LogError(dbUpdateExc.Message, dbUpdateExc.StackTrace);
            return NotFound(dbUpdateExc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return NotFound(exc.Message);
        }
    }

    [Authorize]
    [HttpGet("search/{workOrder}")]
    public async Task<List<WorkOrderSelectDto>> SearchWorkorder(string workOrder)
    {
        try
        {
            return await _repo.SearchWorkOrderAsync(workOrder);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new List<WorkOrderSelectDto>();
        }
    }

    [Authorize]
    [HttpGet("get/for/edit/{workOrderId}")]
    public async Task<WorkOrderEditFormDto> GetForEdit(int workOrderId)
    {
        try
        {
            return await _repo.GetForEditAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new WorkOrderEditFormDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/view/{workOrderId}")]
    public async Task<WorkOrderViewDto> GetForViewId(int workOrderId)
    {
        try
        {
            return await _repo.GetForViewAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new WorkOrderViewDto();
        }
    }

    [Authorize]
    [HttpGet("get/for/delete/{workOrderId}")]
    public async Task<WorkOrderDeleteDto> GetForDeleteId(int workOrderId)
    {
        try
        {
            return await _repo.GetForDeleteAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new WorkOrderDeleteDto();
        }
    }

    [Authorize]
    [HttpGet("get/all")]
    public async Task<List<WorkOrderViewDto>> GetAll()
    {
        try
        {
            return await _repo.GetAllAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc.Message, exc.StackTrace);
            return new List<WorkOrderViewDto>();
        }
    }
}