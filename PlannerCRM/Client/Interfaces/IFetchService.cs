namespace PlannerCRM.Client.Interfaces;

public interface IFetchService<TItem>
    where TItem : class, new()
{
    Task Create(string url, TItem item);
    Task<TItem> Read(string url, int itemId);
    Task Update(string url, TItem item);
    Task Delete(string url, int itemId);
    Task<List<TItem>> GetAll(string url, int offset, int limit);
    Task<List<TItem>> GetAll(string parameterizedUrl);
}
