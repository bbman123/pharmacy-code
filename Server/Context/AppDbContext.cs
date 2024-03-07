using Microsoft.EntityFrameworkCore;
using Shared.Models.Charges;
using Shared.Models.Company;
using Shared.Models.Customers;
using Shared.Models.Labs;
using Shared.Models.Orders;
using Shared.Models.Products;
using Shared.Models.Users;

namespace Server.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Referer> Referers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }    
    public DbSet<Order> Orders { get; set; }        
    public DbSet<ProductOrderItem> OrderItems { get; set; }        
    public DbSet<OrderReferer> OrderReferers { get; set; }        
    public DbSet<LabTest> LabTests  { get; set; }
    public DbSet<LabOrder> LabOrders  { get; set; }
    public DbSet<LabOrderItem> LabOrderItems  { get; set; }
    public DbSet<LabDiagnose> LabDiagonses  { get; set; }
    public DbSet<Charge> Charges  { get; set; }
}
