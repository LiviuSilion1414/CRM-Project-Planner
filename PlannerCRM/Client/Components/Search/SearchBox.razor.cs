namespace PlannerCRM.Client.Components.Search;

public partial class SearchBox : ComponentBase
{
    [Parameter] public EventCallback<string> GetSearchedItems { get; set; }

    private string _query;

    private async Task Search() {
        if (IsValidQuery(_query)) {
            await GetSearchedItems.InvokeAsync(_query);
        }
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