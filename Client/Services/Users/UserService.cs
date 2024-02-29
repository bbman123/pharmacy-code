using Shared.Models.Users;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Services.Users;

public interface IUserService
{
    Task<User?> GetUser(Guid id);
    Task<bool> AddUser(User model);
    Task<bool> EditUser(User model);
    Task<bool> DeleteUser(Guid id);
    Task<User[]?> GetUsers();
}

public class UserService : IUserService
{
    private readonly IHttpClientFactory _client;

    public UserService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddUser(User model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/users", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").DeleteAsync("api/users/{id}");
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditUser(User model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PutAsJsonAsync($"api/users/{model.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<User?> GetUser(Guid id)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<User?>($"api/users/{id}");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<User[]?> GetUsers()
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<User[]?>($"api/users");
        }
        catch (Exception)
        {

            throw;
        }
    }
}
