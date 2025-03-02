namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController(WorkOrderRepository repo) : ControllerBase
{ 
    private readonly WorkOrderRepository _repo = repo;

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
            var workOrder = await _repo.GetByIdAsync(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = workOrder,
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

    [HttpGet]
    [Route(WorkOrderEndpointActions.SEARCH_WORKORDER_BY_TITLE_PLACEHOLDER)]
    public async Task<ResultDto> SearchWorOrderByTitle([FromBody] FilterDto filter)
    {
        try
        {
            var workOrder = await _repo.SearchWorOrderByTitle(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = workOrder,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch(Exception ex) 
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
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedActivitiesByWorkOrderId([FromBody] FilterDto filter)
    {
        try
        {
            var activities = await _repo.FindAssociatedActivitiesByWorkOrderId(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = activities,
                HasCompleted = true,
                Message = "Operation completed",
                MessageType = MessageType.Success,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch(Exception ex) 
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
    [Route(WorkOrderEndpointActions.FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID_PLACEHOLDER)]
    public async Task<ResultDto> FindAssociatedWorkOrdersByClientId([FromBody] FilterDto filter)
    {
        try
        {
            var workOrders = await _repo.FindAssociatedWorkOrdersByClientId(filter);
            return new ResultDto()
            {
                Guid = null,
                Data = workOrders,
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