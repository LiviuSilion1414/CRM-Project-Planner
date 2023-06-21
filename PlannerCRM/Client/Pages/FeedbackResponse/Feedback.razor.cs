using Microsoft.AspNetCore.Components;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.FeedbackResponse;

public partial class Feedback
{
    [Parameter] public string Message { get; set; }
    [Parameter] public FatalityType Severity { get; set; }
    private bool _isXAlertClicked { get; set; }

    private void SwitchShowAlert() {
        if (_isXAlertClicked) {
            _isXAlertClicked = false;
        } else {
            _isXAlertClicked = true;
        }
    }
}