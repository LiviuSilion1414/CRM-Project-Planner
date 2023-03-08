namespace PlannerCRM.Server.Services.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity> Get(int id);
    Task<List<TEntity>> GetAll();
    Task Add(TEntity entity);
    Task<bool> Edit(int id, TEntity entity);
    Task Delete(int id);
}