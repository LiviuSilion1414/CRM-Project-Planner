namespace PlannerCRM.Server.Interfaces;

public interface IRepository<TInput>
    where TInput : class
{
    public Task AddAsync(TInput model);
    public Task EditAsync(TInput model, int id);
    public Task DeleteAsync(int id);
    public Task<TInput> GetByIdAsync(int id);
    public Task<ICollection<TInput>> GetWithPagination(int offset, int limit);
}