namespace PlannerCRM.Client.Components.FeedbackResponse;

public partial class Feedback : ComponentBase
{
    [Parameter] public string Severity { get; set; }
    [Parameter] public string Message { get; set; }

    private bool _isHidden = false;

    private void HideBanner()
        => _isHidden = true;
}