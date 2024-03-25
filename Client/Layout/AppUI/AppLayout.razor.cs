using Client.AppTheme;
using Client.Services.AppService;
using Microsoft.AspNetCore.Components;

namespace Client.Layout.AppUI;

public partial class AppLayout : LayoutComponentBase
{
    [Inject] private LayoutService? LayoutService { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    private bool _drawerOpen = false;
    private bool _topMenuOpen = false;
    protected override async Task OnInitializedAsync()
    {
        LayoutService?.SetBaseTheme(Theme.LandingPageTheme());
        AppState.Token = await localStorage.GetItemAsync<string>("token");
    }

    protected override void OnAfterRender(bool firstRender)
    {
        //refresh nav menu because no parameters change in nav menu but internal data does
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OpenTopMenu()
    {
        _topMenuOpen = true;
    }

    private void OnDrawerOpenChanged(bool value)
    {
        _topMenuOpen = false;
        _drawerOpen = value;
        StateHasChanged();
    }
}
