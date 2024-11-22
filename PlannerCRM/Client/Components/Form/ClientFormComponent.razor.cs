namespace PlannerCRM.Client.Components.Form;

public partial class ClientFormComponent : ComponentBase
{
    [Parameter] public FirmClientDto Client { get; set; }
    [Parameter] public EventCallback<FirmClientDto> UpdatedClient { get; set; }

    private EditContext _context;

    protected override void OnInitialized()
    {
        _context = new(Client);
    }

    private async Task OnSubmit()
    {
        await UpdatedClient.InvokeAsync(Client);
    }
}