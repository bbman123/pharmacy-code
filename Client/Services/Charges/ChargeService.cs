using System.Net.Http.Json;
using Shared.Helpers;
using Shared.Models.Charges;

namespace Client.Services.Charges;

public interface IChargeService
{
    Task<bool> AddCharge(Charge model);
    Task<bool> EditCharge(Charge model);
    Task<bool> DeleteCharge(Guid id);
    Task<Charge?> GetCharge(Guid id);
    Task<Charge[]?> GetCharges();
    Task<GridDataResponse<Charge>?> GetPagedCharges(PaginationParameter parameter);
}

public class ChargeService : IChargeService
{
    private readonly IHttpClientFactory _client;

    public ChargeService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddCharge(Charge model)
    {
        try
        {
            var response = await _client.CreateClient("AppUrl").PostAsJsonAsync("api/charges", model);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteCharge(Guid id)
    {
        try
        {
            var response = await _client.CreateClient("AppUrl").DeleteAsync($"api/charges/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditCharge(Charge model)
    {
        try
        {
            var response = await _client.CreateClient("AppUrl").PutAsJsonAsync($"api/charges/{model!.Id}", model);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Charge?> GetCharge(Guid id)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Charge?>($"api/charges/{id}");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Charge[]?> GetCharges()
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Charge[]?>($"api/charges");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<GridDataResponse<Charge>?> GetPagedCharges(PaginationParameter parameter)
    {
        try
        {
            var response = await _client.CreateClient("AppUrl").PostAsJsonAsync("api/charges/paged", parameter);
            return await response.Content.ReadFromJsonAsync<GridDataResponse<Charge>?>();
        }
        catch (Exception)
        {

            throw;
        }
    }
}
