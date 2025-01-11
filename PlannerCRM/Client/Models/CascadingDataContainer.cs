namespace PlannerCRM.Client.Models;

public class CascadingDataContainer<TItem, TTemp>
    where TItem : class, new()
    where TTemp : class, new()
{
    public bool IsItemSelected { get; set; } = false;
    public bool IsAddSelected { get; set; } = false;
    public bool IsUpdateSelected { get; set; } = false;
    public bool IsDeleteSingleItemSelected { get; set; } = false;
    public bool IsDeleteMultipleSelected { get; set; } = false;
    public bool IsOperationDone { get; set; } = false;
    public string Query { get; set; } = string.Empty;
    public TItem SelectedItem { get; set; } = new();
    public TItem NewItem { get; set; } = new();
    public TTemp TempItem { get; set; } = new();
    public List<string> SelectedProperties { get; set; } = [];
    public List<TItem> MainItems { get; set; } = [];
    public List<TTemp> TempItems { get; set; } = [];
    public List<TItem> SelectedItems { get; set; } = [];
}
