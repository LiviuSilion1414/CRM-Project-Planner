namespace PlannerCRM.Client.Components.MD;

public partial class MasterDetailView<TItem> : ComponentBase
{
    [Parameter] public ICollection<TItem> Data { get; set; } 
    [Parameter] public bool AllowPaging { get; set; }
    [Parameter] public bool AllowFiltering { get; set; }
    [Parameter] public bool AllowSorting { get; set; }
    [Parameter] public List<string> Properties { get; set; }
    [Parameter] public EventCallback<TItem> OnItemSelected { get; set; }
    [Parameter] public EventCallback<bool> OnEditSelected { get; set; }
    [Parameter] public EventCallback<bool> OnDeleteSelected { get; set; }
    [Parameter] public RenderFragment<TItem> AdditionalContent { get; set; }

    private TItem _selectedItem = null;

    private async Task OnRowSelect(TItem item)
    {
        _selectedItem = item;
        await OnItemSelected.InvokeAsync(_selectedItem);
    }

    private async Task OnRowDeselect(TItem item)
    {
        _selectedItem = null;
        await OnItemSelected.InvokeAsync(_selectedItem);
    }

    private bool IsItemSelected()
    {
        return _selectedItem is not null;
    }

    private async Task EditSelected()
    {
        await OnEditSelected.InvokeAsync(IsItemSelected());
    }

    private async Task DeleteSelected()
    {
        await OnDeleteSelected.InvokeAsync(IsItemSelected());
    }
}