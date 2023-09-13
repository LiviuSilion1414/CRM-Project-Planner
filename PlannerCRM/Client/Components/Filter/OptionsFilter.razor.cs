namespace PlannerCRM.Client.Components.Filter;

public partial class OptionsFilter : ComponentBase
{
    [Parameter] public Dictionary<string, Action> Actions { get; set; }

    private string _filterKey;

    protected override void OnInitialized() {
        _filterKey = string.Empty;
    }

    private void HandleFiltering(string key) {
        if (Actions.ContainsKey(key)) {
            Actions[key].Invoke();

            _filterKey = key;
        }
    }
}

internal static class Colors 
{
    public static string BLUE = "blue";
    public static string WHITE = "white";
}