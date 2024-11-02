namespace PlannerCRM.Server.Controllers;

public class CrudController<TEntity>(IRepository<TEntity> repo) : ControllerBase
    where TEntity : class
{
    private readonly IRepository<TEntity> _repo = repo;

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Add(TEntity entity)
    {
        await _repo.AddAsync(entity);

        return Ok();
    }

    [HttpPut]
    [Route("[action]/{entityId:int}")]
    public async Task<IActionResult> Edit(TEntity entity, int entityId)
    {
        await _repo.EditAsync(entity, entityId);

        return Ok();
    }

    [HttpDelete]
    [Route("[action]")]
    public async Task<IActionResult> Delete(int entityId)
    {
        await _repo.DeleteAsync(entityId);

        return Ok();
    }

    [HttpGet]
    [Route("[action]/{entityId:int}")]
    public async Task<ActionResult<TEntity>> GetById(int entityId)
    {
        var entity = await _repo.GetByIdAsync(entityId); //add null interceptor

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpGet]
    [Route("[action]/{limit:int}/{offset:int}")]
    public async Task<ActionResult<ICollection<TEntity>>> GetAll(int limit, int offset)
    {
        var entities = await _repo.GetWithPagination(offset, limit);

        return Ok(entities);
    }
}
