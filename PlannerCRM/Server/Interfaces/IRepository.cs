namespace PlannerCRM.Server.Interfaces;

public interface IRepository<TInput>
    where TInput : class
{
    public Task AddAsync(TInput dto);
    public Task EditAsync(TInput dto);
    public Task DeleteAsync(int id);
}