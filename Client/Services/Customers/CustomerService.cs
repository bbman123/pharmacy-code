using Shared.Helpers;
using Shared.Models.Customers;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Services.Customers;

public interface ICustomerService
{
    Task<bool> AddCustomer(Customer model);
    Task<bool> EditCustomer(Customer model);
    Task<bool> DeleteCustomer(Guid id);
    Task<Customer?> GetCustomerById(Guid id);
    Task<Customer?> GetCustomerByPhone(string phone);
    Task<Customer[]?> GetCustomers();
    Task<GridDataResponse<Customer>> GetPagedCustomers(PaginationParameter parameter);
}
public class CustomerService : ICustomerService
{
    private readonly IHttpClientFactory _client;

    public CustomerService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddCustomer(Customer model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/customers", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteCustomer(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").DeleteAsync($"api/customers/{id}");
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditCustomer(Customer model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PutAsJsonAsync($"api/customers/{model.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Customer?> GetCustomerById(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").GetFromJsonAsync<Customer?>($"api/customers/{id}");
            var response = await request;
            return response;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Customer?> GetCustomerByPhone(string phone)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").GetFromJsonAsync<Customer?>($"api/customers/byPhone/{phone}");
            var response = await request;
            return response;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Customer[]?> GetCustomers()
    {
        try
        {
            var request = _client.CreateClient("AppUrl").GetFromJsonAsync<Customer[]?>($"api/customers");
            var response = await request;            
            return response;
        }
        catch (Exception)
        {

            throw;
        }
    }

	public async Task<GridDataResponse<Customer>> GetPagedCustomers(PaginationParameter parameter)
	{
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/customers/paged", parameter);
            var response = await request;
            var contents= await response.Content.ReadFromJsonAsync<GridDataResponse<Customer>>();
            return contents!;
        }
        catch (Exception)
        {

            throw;
        }
	}
}
