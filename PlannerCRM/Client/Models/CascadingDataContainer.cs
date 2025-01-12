namespace PlannerCRM.Client.Models;

public class CascadingDataContainer<TItem>
    where TItem : class, new()
{
    public ActionStateManager ActionStateManager { get; set; } = new();
    public QueryManager QueryManager { get; set; } = new();
    public DataManager<TItem> DataManager { get; set; } = new();
}
