namespace PlannerCRM.Client.Interfaces;

public interface IFetchService<TItem>
    where TItem : class
{
    Task Create(TItem item);
    Task<TItem> Read(int itemId);
    Task Update(TItem item);
    Task Delete(int itemId);
    Task<ICollection<TItem>> GetAll();
}
