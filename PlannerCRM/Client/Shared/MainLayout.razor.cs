using PlannerCRM.Client.Services.Utilities.Navigation.Lock;

namespace PlannerCRM.Client.Shared;

public partial class MainLayout : LayoutComponentBase
{
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    protected override async Task OnInitializedAsync()
        => await NavigationUtil.HandleAuthenticationAndNavigationAsync();

    public async Task NavigateBasedOnRole() {
        if (!await NavigationUtil.IsUserAuthenticated()) {
            NavManager.NavigateTo(ConstantValues.LOGIN_PAGE_LONG);
        }
    }
}
