using Bogus;
using Server.Context;
using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Charges;
using Shared.Models.Company;
using Shared.Models.Customers;
using Shared.Models.Labs;
using Shared.Models.Products;
using Shared.Models.Users;

namespace Server.Data;

public class SeedData
{
    static DateTime Now = DateTime.Now;
    public static void EnsureSeeded(IServiceProvider services)
    {
        var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //db.Database.EnsureDeleted();
        if (db.Database.EnsureCreated())
        {
            AddUsers(db);
            AddLab(db);
            AddItems(db);
            AddCharges(db);
            AddCustomers(db);
        }
    }

    private static void AddCustomers(AppDbContext db)
    {
        try
        {
            var bogus = new Faker<Customer>().RuleFor(p => p.CustomerName, f => f.Person.FullName).RuleFor(p => p.PhoneNo,
                f => f.Person.Phone.Substring(0, 11)).RuleFor(p => p.ContactAddress, f => f.Person.Address.ToString()).RuleFor(p => p.CreatedDate, f => f.Date.Recent());
            var customers = bogus.Generate(50);
            db.Customers.AddRange(customers);
            db.SaveChanges();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private static void AddCharges(AppDbContext db)
    {
        var charges = new List<Charge>
        {
            new Charge
            {
                Id = Guid.NewGuid(),
                ChargeName = "Escort referal fee",
                Rate = 500,
                Type = RefererType.Escort,
                CreatedDate = Now,
                ModifiedDate = Now
            },
            new Charge
            {
                Id = Guid.NewGuid(),
                ChargeName = "Doctor referal fee",
                Rate = 1000,
                Type = RefererType.Doctor,
                CreatedDate = Now,
                ModifiedDate = Now
            }
        };
        db.Charges.AddRange(charges);
        db.SaveChanges();
    }

    private static void AddItems(AppDbContext db)
    {
        Guid antibiotics = Guid.NewGuid();
        var categories = new List<Category>()
        {
            new Category
            {
                Id = antibiotics,
                CategoryName = "Antibiotics",
                CreatedDate = Now,
                ModifiedDate = Now
            }
        };
        var products = new List<Product>()
        {
            new Product
            {
                Id = Guid.NewGuid(),
                ProductName = "Tetracycline",
                StoreId = StoreID,
                CategoryID = antibiotics,
                StocksOnHand = 10000,
                UnitPrice = 10,
                ReorderLevel = 100,
                ManufacturedDate = DateOnly.FromDateTime(Now),
                ExpiryDate = DateOnly.FromDateTime(Now),
                CreatedDate = Now,
                ModifiedDate = Now,
                Stocks = new List<Stock>()
                {
                    new Stock
                    {
                        Date = Now,
                        Quantity = 10000
                    }
                }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                ProductName = "Amoxicilin",
                StoreId = StoreID,
                CategoryID = antibiotics,
                StocksOnHand = 15000,
                UnitPrice = 15,
                ReorderLevel = 100,
                ManufacturedDate = DateOnly.FromDateTime(Now),
                ExpiryDate = DateOnly.FromDateTime(Now),
                CreatedDate = Now,
                ModifiedDate = Now,
                Stocks = new List<Stock>()
                {
                    new Stock
                    {
                        Date = Now,
                        Quantity = 15000
                    }
                }
            }
        };
        db.Categories.AddRange(categories);
        db.Products.AddRange(products);
        db.SaveChanges();

    }

    private static void AddLab(AppDbContext db)
    {
        var labtest = new List<LabTest>()
        { 
            new LabTest
            {
                Id = Guid.NewGuid(),
                TestName = "Malaria",
                Rate = 1000,
                Type = ServiceType.Lab,
                CreatedDate = Now,
                ModifiedDate = Now,
            },
            new LabTest
            {
                Id = Guid.NewGuid(),
                TestName = "Widal Test",
                Rate = 2000,
                Type = ServiceType.Lab,
                CreatedDate = Now,
                ModifiedDate = Now,
            },
            new LabTest
            {
                Id = Guid.NewGuid(),
                TestName = "Consultation",
                Rate = 1500,
                Type = ServiceType.Consultation,
                CreatedDate = Now,
                ModifiedDate = Now,
            }
        };
        db.LabTests.AddRange(labtest);
        db.SaveChanges();
    }
    static Guid StoreID = Guid.NewGuid();
    private static void AddUsers(AppDbContext context)
    {
        var stores = new Store
        {
            Id = StoreID,
            BranchName = "Mujahid Pharmacy",
            BranchAddress = "115 Ahmadu Bello Way",
            PhoneNo1 = "07068716813",
            CreatedDate = Now,
            ModifiedDate = Now
        };
        var users = new User[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "System",
                LastName = "Administrator",
                Username = "admin",
                HashedPassword = Security.Encrypt("admin123"),
                Role = UserRole.Admin,
                IsActive = true,
                IsNew = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            },
            new User
            {
                Id = Guid.NewGuid(),
                StoreId = StoreID,
                FirstName = "Sell",
                LastName = "Point",
                Username = "seller",
                HashedPassword = Security.Encrypt("seller123"),
                Role = UserRole.Seller,
                IsActive = true,
                IsNew = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            },
            new User
            {
                Id = Guid.NewGuid(),
                StoreId = StoreID,
                FirstName = "Lab",
                LastName = "Scientist",
                Username = "lab",
                HashedPassword = Security.Encrypt("12345678"),
                Role = UserRole.Lab,
                IsActive = true,
                IsNew = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            }
        };
        var referers = new List<Referer>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                RefererName = "Escort",
                Type = RefererType.Escort,
                CreatedDate = Now,
                ModifiedDate = Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                RefererName = "Doctor",
                Type = RefererType.Doctor,
                CreatedDate = Now,
                ModifiedDate = Now
            },
        };
        context.Stores.Add(stores);
        context.Users.AddRange(users);
        context.Referers.AddRange(referers);
        context.SaveChanges();
    }
}
