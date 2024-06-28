namespace PlannerCRM.Server.Interfaces;

public interface IRepository<TInput, TOutput> 
    where TInput : class
    where TOutput : class
{
    public Task AddAsync(TInput dto);
    public Task EditAsync(TInput dto);
    public Task DeleteAsync(int id);
    public Task<TOutput> GetForViewByIdAsync(int id, int id2 = 0, int id3 = 0);
}