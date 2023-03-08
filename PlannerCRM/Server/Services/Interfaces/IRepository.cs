namespace PlannerCRM.Server.Services.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity> GetAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task<bool> EditAsync(int id, TEntity entity);
    Task DeleteAsync(int id);
}