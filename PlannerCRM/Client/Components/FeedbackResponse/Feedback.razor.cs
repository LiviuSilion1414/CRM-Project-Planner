namespace PlannerCRM.Client.Components.FeedbackResponse;

public partial class Feedback : ComponentBase
{
    [Parameter] public string Severity { get; set; }
    [Parameter] public string Message { get; set; }
    [Parameter] public EventCallback<bool> OnClickCancel { get; set; }

    private async Task HideBanner()
        => await OnClickCancel.InvokeAsync(false);
}