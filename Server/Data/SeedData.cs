using Server.Context;
using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Company;
using Shared.Models.Labs;
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
        }
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
                CreatedDate = Now,
                ModifiedDate = Now,
            },
            new LabTest
            {
                Id = Guid.NewGuid(),
                TestName = "Widal Test",
                Rate = 2000,
                CreatedDate = Now,
                ModifiedDate = Now,
            }
        };
        db.LabTests.AddRange(labtest);
        db.SaveChanges();
    }

    private static void AddUsers(AppDbContext context)
    {
        var stores = new Store
        {
            Id = Guid.NewGuid(),
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
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            },
            new User
            {
                Id = Guid.NewGuid(),
                StoreId = stores.Id,
                FirstName = "Sell",
                LastName = "Point",
                Username = "seller",
                HashedPassword = Security.Encrypt("seller123"),
                Role = UserRole.Seller,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            },
            new User
            {
                Id = Guid.NewGuid(),
                StoreId = stores.Id,
                FirstName = "Lab",
                LastName = "Scientist",
                Username = "lab",
                HashedPassword = Security.Encrypt("12345678"),
                Role = UserRole.Seller,
                IsActive = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            }
        };
        context.Stores.Add(stores);
        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
