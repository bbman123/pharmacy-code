using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Models.Orders;

namespace Server.Hubs;

public class SignalRHubs : Hub
{
    private readonly AppDbContext _context;
    private readonly ILogger<SignalRHubs> _logger;

    public SignalRHubs(AppDbContext context, ILogger<SignalRHubs> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Connected: {0}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public async Task UpdateCustomers() => await Clients.All.SendAsync("UpdateCustomers");
    public async Task UpdateReferrers() => await Clients.All.SendAsync("UpdateReferrers");
    public async Task UpdateDealers() => await Clients.All.SendAsync("UpdateDealers");
    public async Task UpdateCategories() => await Clients.All.SendAsync("UpdateCategories");
    public async Task UpdateProducts() => await Clients.All.SendAsync("UpdateProducts");
    public async Task UpdateItems() => await Clients.All.SendAsync("UpdateItems");
    public async Task UpdateItem() => await Clients.All.SendAsync("UpdateItem");
    public async Task UpdateServices() => await Clients.All.SendAsync("UpdateServices");
    public async Task UpdateCharges() => await Clients.All.SendAsync("UpdateCharges");
    public async Task UpdateBillings() => await Clients.All.SendAsync("UpdateBillings");
    public async Task UpdateLabOrders() => await Clients.All.SendAsync("UpdateLabOrders");
    public async Task UpdatePharmacyOrders() => await Clients.All.SendAsync("UpdatePharmacyOrders");

    public async Task UpdateProductQuantity(ProductOrderItem[] items)
    {
        _logger.LogInformation("Background update in progress...");
        //if (!Context.User!.Identity!.IsAuthenticated)
        //{
        //    _logger.LogError("User not authenticated");
        //    return;
        //}            
        foreach (var item in items)
        {
            _logger.LogInformation("Updating product quantity");
            await _context.Products.Where(x => x.Id == item.ProductId).ExecuteUpdateAsync(s => s.SetProperty(qty => qty.StocksOnHand, q => q.StocksOnHand - (decimal)item.Quantity));
            _logger.LogInformation("Successfully Updated {0} Quantity", item.Product);
        }
    }
}
