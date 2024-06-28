namespace PlannerCRM.Server.Interfaces;

public interface IRepository<TInput, TOutput> 
    where TInput : class
    where TOutput : class
{
    public Task AddAsync(TInput dto);
    public Task EditAsync(TInput dto);
    public Task DeleteAsync(int id);
    public Task<TOutput> GetForViewByIdAsync(int id);
    public Task<TOutput> GetForEditByIdAsync(int id);
    public Task<TOutput> GetForDeleteByIdAsync(int id);
    public Task<List<TOutput>> GetPaginatedDataAsync(int limit, int offset);
}