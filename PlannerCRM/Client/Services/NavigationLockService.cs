using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace PlannerCRM.Client.Services;

public class NavigationLockService
{
    private readonly IJSRuntime _js;
    public bool ConfirmedExternalExit { get { return true; } }
    
    public NavigationLockService(IJSRuntime js) {
        _js = js;
    }

    public async Task ConfirmInternalExit(LocationChangingContext context) {
        var confirmed = await _js.InvokeAsync<bool>("window.confirm", "Changes that you made may not be saved. Continue?");

        if (!confirmed) {
            context.PreventNavigation();
        }
    }
}
