using Shared.Helpers;
using Shared.Models.Labs;
using Shared.Models.Orders;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Services.Orders;

public interface IOrderService
{
    Task<bool> AddProductOrder(Order model);
    Task<bool> AddLabOrder(LabOrder model);
    Task<GridDataResponse<OrderWithData>?> GetOrderByStore(string Type, PaginationParameter parameter);
}
public class OrderService : IOrderService
{
    private readonly IHttpClientFactory _client;

    public OrderService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddLabOrder(LabOrder model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/laborders", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> AddProductOrder(Order model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/orders", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task<GridDataResponse<OrderWithData>?> GetOrderByStore(string Type, PaginationParameter parameter)
    {
        HttpResponseMessage? response = null;
        try
        {
            if (Type == "Lab")
                response = await _client.CreateClient("AppUrl").PostAsJsonAsync("api/laborders/page", parameter);
            else
                response = await _client.CreateClient("AppUrl").PostAsJsonAsync("api/orders/page", parameter);

            return await response.Content.ReadFromJsonAsync<GridDataResponse<OrderWithData>>();
        }
        catch (Exception)
        {

            throw;
        }
    }
}
