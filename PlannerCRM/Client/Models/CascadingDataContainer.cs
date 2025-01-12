namespace PlannerCRM.Client.Models;

public class CascadingDataContainer<TItem, TTemp>
    where TItem : class, new()
    where TTemp : class, new()
{
    public ActionStateManager ActionStateManager { get; set; } = new();
    public QueryManager QueryManager { get; set; } = new();
    public DataManager<TItem, TTemp> DataManager { get; set; } = new();
}
