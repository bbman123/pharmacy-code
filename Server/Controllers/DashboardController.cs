using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Shared.Models.Dashboard;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<DashboardModel?>> GetDashboardData()
        {
            DashboardModel? model = new();

            model.TotalLabSales = await _context.LabOrders.CountAsync();
            model.TotalPharmacySales = await _context.Orders.CountAsync();
            model.TotalCustomers = await _context.Customers.CountAsync();
            //

            model.BranchSales = await _context.Stores.Include(x => x.Orders).Include(x => x.LabOrders).GroupBy(x => x.Id).Select(x => new BranchSalesChart
            {
                Branch = x.FirstOrDefault(y => y.Id == x.Key)!.BranchName,
                LabSales = x.Select(y => y.LabOrders).Count(),
                PharmacySales = x.Select(y => y.Orders).Count()
            }).ToArrayAsync();

            //model.BranchNetSales = await _context.

            return model;

        }
    }
}
