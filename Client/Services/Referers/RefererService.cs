using Shared.Models.Users;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Services.Referers;

public interface IRefererService
{
    Task<bool> AddReferer(Referer model);
    Task<bool> EditReferer(Referer model);
    Task<bool> DeleteReferer(Guid id);
    Task<Referer?> GetReferer(Guid id);
    Task<Referer[]?> GetReferers();
}
public class RefererService : IRefererService
{
    private readonly IHttpClientFactory _client;

    public RefererService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddReferer(Referer model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/referers", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteReferer(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").DeleteAsync($"api/referers/{id}");
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditReferer(Referer model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PutAsJsonAsync($"api/referers/{model.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Referer?> GetReferer(Guid id)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Referer?>($"api/referers/{id}");            
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Referer[]?> GetReferers()
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Referer[]?>($"api/referers");
        }
        catch (Exception)
        {

            throw;
        }
    }
}

