namespace PlannerCRM.Client.Components.ModalForm.Confirm;
public partial class ConfirmationModal : ComponentBase
{
    [Parameter] public string Title { get; set; } = Titles.CONFIRM_ACTION;
    [Parameter] public string ConfirmationMessage { get; set; } = WarningMessages.CONFIRMATION_MESSAGE;
    [Parameter] public string ConfirmButtonText { get; set; } = ConfirmMessages.CONFIRM;
    [Parameter] public EventCallback<bool> SendConfirmation { get; set; }

    private Dictionary<string, object> Attributes = new() { { "z-index", "10" } };
    private bool _isCancelClicked = false;

    public async Task ConfirmAction()
    {
        CloseModal();
        await SendConfirmation.InvokeAsync(true);
    }

    private void CloseModal()
        => _isCancelClicked = !_isCancelClicked;
}