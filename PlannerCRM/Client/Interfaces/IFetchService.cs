namespace PlannerCRM.Client.Interfaces;

public interface IFetchService<TItem>
    where TItem : class, new()
{
    Task Create(string controllerName, string url, TItem item);
    Task<TItem> Read(string controllerName, string url, int itemId);
    Task Update(string controllerName, string url, TItem item);
    Task Delete(string controllerName, string url, int itemId);
    Task<List<TItem>> GetAll(string controllerName, string url, int offset, int limit);
    Task<List<TItem>> GetAll(string controllerName, string parameterizedUrl);
}
