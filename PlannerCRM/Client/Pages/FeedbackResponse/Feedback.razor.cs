using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Pages.AccountManager.Add;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Client.Pages.FeedbackResponse;

public partial class Feedback
{
    [Parameter] public string Message { get; set; }
    [Parameter] public bool Hidden { get; set; }

    protected override void OnInitialized() => Hidden = false;

    private void SwitchShowAlert() {
        Hidden = !Hidden;
    }
}