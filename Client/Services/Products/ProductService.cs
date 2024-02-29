using Shared.Helpers;
using Shared.Models.Products;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Client.Services.Products;

public interface IProductService
{
    Task<bool> AddCategory(Category model);
    Task<bool> EditCategory(Category model);
    Task<bool> DeleteCategory(Guid id);
    Task<Category?> GetCategoryById(Guid id);
    Task<Category[]?> GetCategories();
    Task<GridDataResponse<Category>?> GetPagedCategories(PaginationParameter parameter);
    
    Task<bool> AddProduct(Product model);
    Task<bool> EditProduct(Product model);
    Task<bool> DeleteProduct(Guid id);
    Task<Product?> GetProductById(Guid id);
    Task<Product[]?> GetProducts();
	Task<GridDataResponse<Product>?> GetPagedProducts(PaginationParameter parameter);
}

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _client;

    public ProductService(IHttpClientFactory client)
    {
        _client = client;
    }

    public async Task<bool> AddCategory(Category model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/categories", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> AddProduct(Product model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PostAsJsonAsync("api/products", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteCategory(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").DeleteAsync($"api/categories/{id}");
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").DeleteAsync($"api/products/{id}");
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditCategory(Category model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PutAsJsonAsync($"api/categories/{model.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> EditProduct(Product model)
    {
        try
        {
            var request = _client.CreateClient("AppUrl").PutAsJsonAsync($"api/products/{model.Id}", model);
            var response = await request;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Category[]?> GetCategories()
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Category[]?>("api/categories");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Category?> GetCategoryById(Guid id)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Category?>($"api/categories/{id}");
        }
        catch (Exception)
        {

            throw;
        }
    }

	public async Task<GridDataResponse<Category>?> GetPagedCategories(PaginationParameter parameter)
	{
		try
		{
			var request = await  _client.CreateClient("AppUrl").PostAsJsonAsync($"api/categories/paged",parameter);
            var response = await request.Content.ReadFromJsonAsync<GridDataResponse<Category>?>();
            return response;
		}
		catch (Exception)
		{

			throw;
		}
	}

	public async Task<GridDataResponse<Product>?> GetPagedProducts(PaginationParameter parameter)
	{
		try
		{
			var request = await _client.CreateClient("AppUrl").PostAsJsonAsync($"api/products/paged", parameter);
			var response = await request.Content.ReadFromJsonAsync<GridDataResponse<Product>?>();
			return response!;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task<Product?> GetProductById(Guid id)
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Product?>($"api/products/{id}");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Product[]?> GetProducts()
    {
        try
        {
            return await _client.CreateClient("AppUrl").GetFromJsonAsync<Product[]?>("api/products");
        }
        catch (Exception)
        {

            throw;
        }
    }
}
