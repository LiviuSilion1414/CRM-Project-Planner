using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace PlannerCRM.Client.Services;

public class NavigationLockService
{
    private readonly IJSRuntime _js;
    public readonly bool ConfirmedExternalExit = true;
    
    public NavigationLockService(IJSRuntime js) {
        _js = js;
    }

    public async Task ConfirmInternalSite(LocationChangingContext context) {
        var confirmed = await _js.InvokeAsync<bool>("window.confirm", "I dati inseriti non sono salvati. Vuoi continuare?");

        if (!confirmed) {
            context.PreventNavigation();
        }
    }
}
