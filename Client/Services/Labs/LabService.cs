using Microsoft.VisualBasic;
using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Labs;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Services.Labs;

public interface ILabService
{
    Task<bool> AddLabTest(LabTest model);
    Task<bool> EditLabTest(LabTest model);
    Task<bool> DeleteLabTest(Guid id);
    Task<LabTest?> GetLabTest(Guid id);
    Task<LabDiagnose?> GetOrderDiagnose(Guid id);
    Task<LabTest[]?> GetLabTests();
    Task<LabTest[]?> GetLabTests(ServiceType type);
    Task<GridDataResponse<LabTest>?> GetPagedTests(PaginationParameter parameter);
}
public class LabService : ILabService
{
    private readonly IHttpClientFactory _client;

    public LabService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddLabTest(LabTest model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/labtests", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteLabTest(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").DeleteAsync($"api/labtests/{id}");
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditLabTest(LabTest model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync($"api/labtests/{model.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<LabTest?> GetLabTest(Guid id)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<LabTest?>($"api/labtests/{id}");           
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<LabTest[]?> GetLabTests()
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<LabTest[]?>($"api/labtests");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<LabTest[]?> GetLabTests(ServiceType type)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<LabTest[]?>($"api/labtests/type/{type}");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<LabDiagnose?> GetOrderDiagnose(Guid id)
    {
        LabDiagnose? diagnose;
        try
        {
            diagnose = await _client.CreateClient("AppUrl").GetFromJsonAsync<LabDiagnose?>($"api/diagnosis/investigations/{id}");
        }
        catch (Exception)
        {

            throw;
        }
        return diagnose;
    }

    public async Task<GridDataResponse<LabTest>?> GetPagedTests(PaginationParameter parameter)
    {
		try
		{
			var response = await _client.CreateClient("AppUrl").PostAsJsonAsync($"api/labtests/paged", parameter);
            return await response.Content.ReadFromJsonAsync<GridDataResponse<LabTest>>();
		}
		catch (Exception)
		{

			throw;
		}
	}
}
