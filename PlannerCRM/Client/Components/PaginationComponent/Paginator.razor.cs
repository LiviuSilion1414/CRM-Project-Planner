namespace PlannerCRM.Client.Components.PaginationComponent;

public partial class Paginator : ComponentBase 
{
    [Parameter] public int CollectionSize { get; set; }
    [Parameter] public EventCallback<(int, int)> Paginate { get; set; }

    private readonly int _offset = 5;

    private int _totalPageNumbers;
    private int _pageNumber = 1;
    private int _limit;
    
    protected override void OnInitialized() => _totalPageNumbers = CollectionSize / _offset;

    private async Task Previous() {
        if (_limit <= _offset) {
            _limit = 0;
            _pageNumber = 1;
        } else {
            _limit -= _offset;
            _pageNumber--;
        }
       await InvokeCallbackAsync(Paginate, _limit, _offset);
    }

    private async Task Next() {
        if (_limit < _totalPageNumbers + _offset) {
            _limit += _offset;
            _pageNumber++; 
        }
       
       await InvokeCallbackAsync(Paginate, _limit, _offset);
    }

    private static async Task InvokeCallbackAsync(EventCallback<(int, int)> callback, int limit, int offset) 
        => await callback.InvokeAsync((limit, offset));
}