namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.WORKORDER_CONTROLLER)]
public class WorkOrderController(WorkOrderRepository repo) : ControllerBase
{ 
    private readonly WorkOrderRepository _repo = repo;

    [HttpPost]
    [Route(ApiUrl.WORKORDER_INSERT)]
    public async Task<ResultDto> Insert([FromBody] WorkOrderDto dto)
    {
        try
        {
            await _repo.Insert(dto);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        }
        catch  
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPut]
    [Route(ApiUrl.WORKORDER_UPDATE)]
    public async Task<ResultDto> Update(WorkOrderDto dto)
    {
        try
        {
            await _repo.Update(dto);
            return new ResultDto() 
            { 
                id = null, 
                data = null, 
                hasCompleted = true, 
                message = "Operation completed", 
                messageType = MessageType.Success, 
                statusCode = HttpStatusCode.OK 
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.WORKORDER_DELETE)]
    public async Task<ResultDto> Delete([FromBody] WorkOrderFilterDto filter)
    {
        try
        {
            await _repo.Delete(filter);
            return new ResultDto() 
            { 
                id = null, 
                data = null, 
                hasCompleted = true, 
                message = "Operation completed", 
                messageType = MessageType.Success, 
                statusCode = HttpStatusCode.OK 
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.WORKORDER_GET)]
    public async Task<ResultDto> Get([FromBody] WorkOrderFilterDto filter)
    {
        try
        {
            var workOrder = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = workOrder,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.WORKORDER_LIST)]
    public async Task<ResultDto> List([FromBody] WorkOrderFilterDto filter)
    {

        try
        {
            var entities = await _repo.List(filter);
            return new ResultDto()
            {
                id = null,
                data = entities,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };

        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.WORKORDER_SEARCH)]
    public async Task<ResultDto> Search([FromBody] WorkOrderFilterDto filter)
    {
        try
        {
            var workOrder = await _repo.Search(filter);
            return new ResultDto()
            {
                id = null,
                data = workOrder,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        }
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.WORKORDER_FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID)]
    public async Task<ResultDto> FindAssociatedActivitiesByWorkOrderId([FromBody] WorkOrderFilterDto filter)
    {
        try
        {
            var activities = await _repo.FindAssociatedActivitiesByWorkOrderId(filter);
            return new ResultDto()
            {
                id = null,
                data = activities,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        }
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.WORKORDER_FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID)]
    public async Task<ResultDto> FindAssociatedWorkOrdersByClientId([FromBody] WorkOrderFilterDto filter)
    {
        try
        {
            var workOrders = await _repo.FindAssociatedWorkOrdersByClientId(filter);
            return new ResultDto()
            {
                id = null,
                data = workOrders,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }
}