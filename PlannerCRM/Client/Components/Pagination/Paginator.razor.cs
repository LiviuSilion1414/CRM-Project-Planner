namespace PlannerCRM.Client.Components.Pagination;

public partial class Paginator : ComponentBase 
{
    [Parameter] public int CollectionSize { get; set; }
    [Parameter] public EventCallback<(int, int)> Paginate { get; set; }

    private List<int> _pages;
    private readonly int _elementsToTake = 5;
    private int _elementsToSkip;
    private int _pageNumber;
    private int _slices;
    
    protected override void OnInitialized() {
        _elementsToSkip = 0;
        _slices = CollectionSize / _elementsToTake;
        _pages = Enumerable
            .Range(1, _slices)
            .ToList();
        _pageNumber = _pages
            .First();
    }

    private string SetClass(int pageNumber) {
        return pageNumber == _pageNumber
            ? CssClass.Selected
            : CssClass.Empty;
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
        if (_elementsToSkip < CollectionSize && (_elementsToSkip + _elementsToTake < CollectionSize)) {
            _elementsToSkip += _elementsToTake;
            _pageNumber++; 
        }
       
       await InvokeCallbackAsync(Paginate, _elementsToSkip, _elementsToTake);
    }

    private static async Task InvokeCallbackAsync(EventCallback<(int, int)> callback, int skip, int take) 
        => await callback.InvokeAsync((skip, take));
}