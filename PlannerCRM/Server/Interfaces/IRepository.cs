namespace PlannerCRM.Server.Interfaces;

public interface IRepository<T> 
    where T : class
{
    public async Task AddAsync(T dto);
    public async Task EditAsync(T dto);
    public async Task DeleteAsync(int id);
    public async Task<TOutput> GetForViewByIdAsync(int id);
    public async Task<TOutput> GetForEditByIdAsync(int id);
    public async Task<TOutput> GetForDeleteByIdAsync(int id);
    public async Task<List<TOutput>> GetPaginatedDataAsync(int limit, int offset);
}