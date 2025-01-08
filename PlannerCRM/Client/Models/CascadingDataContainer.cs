namespace PlannerCRM.Client.Models;

public class CascadingDataContainer<TItem>
    where TItem : class, new()
{
    public bool IsItemSelected { get; set; }
    public bool IsAddSelected { get; set; }
    public bool IsUpdateSelected { get; set; }
    public bool IsDeleteSingleItemSelected { get; set; }
    public bool IsDeleteMultipleSelected { get; set; }
    public bool IsOperationDone { get; set; }
    public TItem SelectedItem { get; set; }
    public IEnumerable<TItem> SelectedItems { get; set; }
    public TItem NewItem { get; set; }
}
