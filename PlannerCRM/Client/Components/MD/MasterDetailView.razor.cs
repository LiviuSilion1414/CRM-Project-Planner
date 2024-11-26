namespace PlannerCRM.Client.Components.MD;

public partial class MasterDetailView<TItem> : ComponentBase
{
    [Parameter] public ICollection<TItem> Data { get; set; } 
    [Parameter] public DataGridSelectionMode SelectionMode { get; set; }
    [Parameter] public bool AllowPaging { get; set; }
    [Parameter] public bool AllowFiltering { get; set; }
    [Parameter] public bool AllowSorting { get; set; }

    [Parameter] public IEnumerable<string> Columns { get; set; }


    private bool _isEditSelected = false;
    private bool _isDeleteSelected = false;

    private bool _isRowSelected = false;
    private TItem _selectedItem;

    private void OnRowSelect(TItem item)
    {
        _isRowSelected = !_isRowSelected;

        if (_isRowSelected)
        {
            _selectedItem = item;
        } else
        {
            _selectedItem = default;
        }
    }

    private void OnUpdatedClient(TItem item)
    {
        _selectedItem = item;
    }

    private void OnDeleteConfirmed(bool isConfirmed)
    {
        if (isConfirmed)
        {
            Data.Remove(_selectedItem);
        }
    }

    private async Task OnEdit()
    {
        _isEditSelected = !_isEditSelected;
    }

    private async Task OnDelete()
    {
        _isDeleteSelected = !_isDeleteSelected;
    }

    private string GetPropertyValue(string propertyName)
    {
        var propertyInfo = _selectedItem.GetType().GetProperty(propertyName)
            ??throw new InvalidOperationException($"Property with name:'{propertyName}' does not exist in '{typeof(TItem).Name}' type.");

        var value = propertyInfo.GetValue(_selectedItem);
        return value?.ToString() ?? string.Empty;
    }
}