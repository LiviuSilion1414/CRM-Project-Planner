namespace PlannerCRM.Client.Components.PaginationComponent;

public partial class Paginator : ComponentBase 
{
    [Parameter] public int CollectionSize { get; set; }
    [Parameter] public EventCallback<(int, int)> Paginate { get; set; }

    private readonly int _elementsToTake = 5;
    private int _elementsToSkip;
    private int _totalPageNumbers;
    private int _pageNumber;
    private double _result; 
    
    protected override void OnInitialized() {
        _elementsToSkip = 0;
        _pageNumber = 1;
        _result = CollectionSize % _elementsToTake;
        _totalPageNumbers = _elementsToTake > CollectionSize
            ? 1
            : ((int) Math.Ceiling(_result));
    }

    private async Task Previous() {
        if (_elementsToSkip <= _elementsToTake) {
            _elementsToSkip = 0;
            _pageNumber = 1;
        } else {
            _elementsToSkip -= _elementsToTake;
            _pageNumber--;
        }

        await InvokeCallbackAsync(Paginate, _elementsToSkip, _elementsToTake);
    }

    private async Task Next() {
        if (_elementsToSkip <= (_totalPageNumbers + _elementsToTake)) {
            _elementsToSkip += _elementsToTake;
            _pageNumber++; 
        }
       
       await InvokeCallbackAsync(Paginate, _elementsToSkip, _elementsToTake);
    }

    private static async Task InvokeCallbackAsync(EventCallback<(int, int)> callback, int skip, int take) 
        => await callback.InvokeAsync((skip, take));
}