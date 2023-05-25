using Microsoft.AspNetCore.Components;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.FeedbackResponse;

public partial class Feedback
{
    [Parameter] public string Message { get; set; }
    [Parameter] public BannerType Severity { get; set; }
    private bool _isXAlertClicked { get; set; }
    private string _inputType { get; set; }

    private void SwitchShowAlert() {
        if (_isXAlertClicked) {
            _isXAlertClicked = false;
            _inputType = ShowType.NONE.ToString().ToLower();
        } else {
            _isXAlertClicked = true;
            _inputType = ShowType.BLOCK.ToString().ToLower();
        }
    }
}