namespace PlannerCRM.Client.Pages.FeedbackResponse;

public partial class Feedback : ComponentBase
{
    [Parameter] public string Message { get; set; }
    [Parameter] public EventCallback<bool> OnClickCancel { get; set; }

    private void SwitchShowAlert() => OnClickCancel.InvokeAsync(false);
}