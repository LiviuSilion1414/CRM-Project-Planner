namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.MENU_ROLE_CONTROLLER)]
public class MenuRoleController(MenuRoleRepository repo) : ControllerBase
{
    private readonly MenuRoleRepository _repo = repo;

    [HttpPost]
    [Route(ApiUrl.MENU_ROLE_INSERT)]
    public async Task<ResultDto> Insert(MenuRoleDto dto)
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
        } catch
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
    [Route(ApiUrl.MENU_ROLE_UPDATE)]
    public async Task<ResultDto> Update(MenuRoleDto dto)
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
        } catch
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
    [Route(ApiUrl.MENU_ROLE_DELETE)]
    public async Task<ResultDto> Delete(MenuRoleFilterDto filter)
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
        } catch
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
    [Route(ApiUrl.MENU_ROLE_GET)]
    public async Task<ResultDto> Get(MenuRoleFilterDto filter)
    {
        try
        {
            var role = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = role,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch
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
    [Route(ApiUrl.MENU_ROLE_LIST)]
    public async Task<ResultDto> List(MenuRoleFilterDto filter)
    {
        try
        {
            var roles = await _repo.List(filter);
            return new ResultDto()
            {
                id = null,
                data = roles,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception exc)
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
