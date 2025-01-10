namespace PlannerCRM.Server.Interfaces;

public interface IRepository<TInput, TOutput>
    where TInput : class
    where TOutput : class
{
    Task AddAsync(TInput model);

    Task EditAsync(TInput model, int id);

    Task DeleteAsync(int id);

    Task<TOutput> GetByIdAsync(int id);

    Task<ICollection<TOutput>> GetWithPagination(int limit, int offset);
}
