using Shared.Models.Notifications;
using Client.Services.AppService;
using Microsoft.AspNetCore.Components;

namespace Client.Layout.AppUI;

public partial class AppBarButtons
{
    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private LayoutService LayoutService { get; set; } = default!;
    private NotificationMessage[] _messages = null!;
    private bool _newNotificationsAvailable = false;

    private async Task GetNotifications()
    {
        _newNotificationsAvailable = await NotificationService.AreNewNotificationsAvailable();
        _messages = await NotificationService.GetUnReadNotifications();
    }

    private async Task MarkNotificationAsRead()
    {
        await NotificationService.MarkNotificationsAsRead();
        _newNotificationsAvailable = false;
    }
    private async Task LogOut()
    {
        await localStorage.RemoveItemAsync("token");
        await localStorage.RemoveItemAsync("uid");
        await localStorage.RemoveItemAsync("branch");
        await localStorage.RemoveItemAsync("access");
        nav.NavigateTo("/", true);
    }
}