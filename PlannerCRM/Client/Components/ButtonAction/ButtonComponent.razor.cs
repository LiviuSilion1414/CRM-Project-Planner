namespace PlannerCRM.Client.Components.ButtonAction;

public partial class ButtonComponent : ComponentBase
{
    [Parameter] public string Type { get; set; }
    [Parameter] public string Class { get; set; }
    [Parameter] public string InnerText { get; set; }
    [Parameter] public Action RunMethod { get; set; }
}