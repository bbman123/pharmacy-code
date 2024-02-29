using Client.AppTheme;
using Client.Services.AppService;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Client.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] IJSRuntime jsRuntime { get; set; } = default!;
    [Inject] public LayoutService? LayoutService { get; set; }
    public MudThemeProvider _mudThemeProvider = default!;
    // [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    protected override void OnInitialized()
    {        
        //PaintInterop.jsRuntime = jsRuntime;
        LayoutService?.SetBaseTheme(Theme.DocsTheme());
        LayoutService!.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccured!;
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (firstRender)
        {
            await ApplyUserPreferences();
            StateHasChanged();
        }
    }

    private async Task ApplyUserPreferences()
    {
        var defaultDarkMode = await _mudThemeProvider!.GetSystemPreference();
        await LayoutService?.ApplyUserPreferences(defaultDarkMode!)!;
    }

    public void Dispose()
    {
        LayoutService!.MajorUpdateOccured -= LayoutServiceOnMajorUpdateOccured!;
    }

    private void LayoutServiceOnMajorUpdateOccured(object sender, EventArgs e) => StateHasChanged();
}

