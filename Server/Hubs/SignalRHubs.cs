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
