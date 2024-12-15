namespace PlannerCRM.Client.Components.MD;

public partial class MasterDetailView<TItem> : ComponentBase
{
    [Parameter] public ICollection<TItem> Data { get; set; }
    [Parameter] public bool AllowPaging { get; set; }
    [Parameter] public bool AllowFiltering { get; set; }
    [Parameter] public bool AllowSorting { get; set; }
    [Parameter] public bool IsReloadRequired { get; set; }
    [Parameter] public List<string> Properties { get; set; }
    [Parameter] public EventCallback<TItem> OnItemSelected { get; set; }
    [Parameter] public EventCallback<bool> OnEditSelected { get; set; }
    [Parameter] public EventCallback<bool> OnDeleteSelected { get; set; }
    [Parameter] public EventCallback<int> DisplayCount { get; set; }
    [Parameter] public RenderFragment<TItem> AdditionalContent { get; set; }

    private RadzenDataGrid<TItem> _grid;
    private TItem _selectedItem = null;
    private int _count = 0;

    protected override async Task OnInitializedAsync()
    {
        _grid = new();
        _count = (int)ItemsCount.T25;
        await SetDisplayCount(_count);
    }

    protected override void OnParametersSet()
    {
        if (IsReloadRequired)
        {
            _grid.Reload();
        }
    }

    private async Task SetDisplayCount(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int displayCount))
        {
            Console.WriteLine($"display count selected: {displayCount}");
            await DisplayCount.InvokeAsync(displayCount);
        }
    }

    private async Task SetDisplayCount(int displayCount) => await DisplayCount.InvokeAsync(displayCount);

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

    private bool IsItemSelected() => _selectedItem is not null;

    private async Task EditSelected() => await OnEditSelected.InvokeAsync(IsItemSelected());

    private async Task DeleteSelected() => await OnDeleteSelected.InvokeAsync(IsItemSelected());
}