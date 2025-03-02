namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeController(WorkTimeRepository repo) : ControllerBase
{
    private readonly WorkTimeRepository _repo = repo;

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.ADD_PLACEHOLDER)]
    public async Task<ResultDto> Add([FromBody] FilterDto filter)
    {
        try
        {
            await _repo.AddAsync(filter);
            return new ResultDto() 
            { 
                Guid = null, 
                Data = null, 
                HasCompleted = true, 
                Message = "Operation completed", 
                MessageType = MessageType.Success, 
                StatusCode = HttpStatusCode.OK 
            };
        } 
        catch (Exception ex)
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPut]
    [Route(EndpointsCrudPlaceholders.EDIT_PLACEHOLDER)]
    public async Task<ResultDto> Edit([FromBody] FilterDto filter)
    {
        try
        {
            await _repo.EditAsync(filter);
            return new ResultDto() 
            { 
                Guid = null, 
                Data = null, 
                HasCompleted = true, 
                Message = "Operation completed", 
                MessageType = MessageType.Success, 
                StatusCode = HttpStatusCode.OK 
            };
        }
        catch (Exception ex) 
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(EndpointsCrudPlaceholders.DELETE_PLACEHOLDER)]
    public async Task<ResultDto> Delete([FromBody] FilterDto filter)
    {
        try
        {
            await _repo.DeleteAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        } 
        catch (Exception ex) 
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_BY_ID_PLACEHOLDER)]
    public async Task<ResultDto> GetById([FromBody] FilterDto filter)
    {
        try
        {
            var workTime = await _repo.GetByIdAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = workTime,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception ex) 
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpGet]
    [Route(EndpointsCrudPlaceholders.GET_WITH_PAGINATION_PLACEHOLDER)]
    public async Task<ResultDto> GetWithPagination([FromBody] FilterDto filter)
    {
        try
        {
            var entities = await _repo.GetWithPagination(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = entities,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception ex) 
        {
            return new ResultDto()
            {
                Guid = null,
                Data = null,
                HasCompleted = false,
                Message = "Operation failed",
                MessageType = MessageType.Error,
                StatusCode = HttpStatusCode.NotFound
            };
        }

    }
}