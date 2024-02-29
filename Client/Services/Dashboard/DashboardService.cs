using Shared.Models.Dashboard;
using System.Net.Http.Json;

namespace Client.Services.Dashboard;

public interface IDashboardService
{
    Task<DashboardModel?> GetDashboardDataAsync();
}

public class DashboardService : IDashboardService
{
    private readonly IHttpClientFactory _client;

    public DashboardService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<DashboardModel?> GetDashboardDataAsync()
    {
        try
        {
            var request = _client.CreateClient("AppUrl").GetFromJsonAsync<DashboardModel?>("api/dashboard");
            var response = await request;
            return response;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
