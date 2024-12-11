namespace PlannerCRM.Server.Controllers;

[ApiController]
public class CrudController<TInput, TOutput>(IRepository<TInput, TOutput> repo) : ControllerBase
    where TInput : class
    where TOutput : class
{
    private readonly IRepository<TInput, TOutput> _repo = repo;

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(TInput entity)
    {
        await _repo.AddAsync(entity);

        return Ok();
    }

    [HttpPut]
    [Route("edit/{entityId}")]
    public async Task<IActionResult> Edit(TOutput entity, int entityId)
    {
        await _repo.EditAsync(entity, entityId);

        return Ok();
    }

    [HttpDelete]
    [Route("delete/{entityId}")]
    public async Task<IActionResult> Delete(int entityId)
    {
        await _repo.DeleteAsync(entityId);

        return Ok();
    }

    [HttpGet]
    [Route("getById/{entityId}")]
    public async Task<ActionResult<TOutput>> GetById(int entityId)
    {
        var entity = await _repo.GetByIdAsync(entityId); //add null interceptor

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpGet]
    [Route("getWithPagination/{limit}/{offset}")]
    public async Task<ActionResult<ICollection<TOutput>>> GetWithPagination(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(limit, offset);

        return Ok(entities);
    }
}
