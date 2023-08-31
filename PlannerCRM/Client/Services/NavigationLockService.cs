using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace PlannerCRM.Client.Services;

public class NavigationLockService
{
    private readonly IJSRuntime _js;
    private NavigationManager _navigationManager;

    public const bool ConfirmedExternalExit = true;

    public NavigationLockService(IJSRuntime js, NavigationManager navigationManager) {
        _js = js;
        _navigationManager = navigationManager;
    }
    
    public async Task ConfirmInternalExit(LocationChangingContext context) {
        var confirmed = await _js.InvokeAsync<bool>("window.confirm", "Changes that you made may not be saved. Continue?");

        if (!confirmed) {
            context.PreventNavigation();
        }
    }

    public string GetCurrentPage() {
        return _navigationManager.Uri.Replace(_navigationManager.BaseUri, "/");
    }

    public string BuildNavigationUrl(string role, int employeeId) {
        if (!string.IsNullOrEmpty(role) && Enum.TryParse(role, out Roles parsedRole)) {
            var url = parsedRole
                .ToString()
                .ToLower()
                .Replace('_', '-');

            if (parsedRole == Roles.SENIOR_DEVELOPER || parsedRole == Roles.JUNIOR_DEVELOPER) {
                url += $"/{employeeId}";
            }

            return url;
        }

        return string.Empty;
    }
}
