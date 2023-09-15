namespace PlannerCRM.Client.Components.FilterAndSearch.Search;

public partial class SearchBox : ComponentBase
{
    [Parameter] public EventCallback<string> GetSearchedItems { get; set; }

    private string _query;

    protected override void OnInitialized() {
        _query = string.Empty;
    }

    private async Task Search() {
        if (IsValidQuery(_query)) {
            await GetSearchedItems.InvokeAsync(_query);
        }
        
        await GetSearchedItems.InvokeAsync(string.Empty);
    }

    private static bool IsValidQuery(string query) {
        return 
            !string.IsNullOrEmpty(query) &&
            !query
                .Any(c => 
                    char.IsDigit(c) || 
                    char.IsSymbol(c) || 
                    char.IsWhiteSpace(c)
                );
    }
}