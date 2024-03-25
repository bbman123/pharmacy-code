using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models.Orders;
using Shared.Models.Company;

namespace Shared.Models.Products;

public class Product
{
    [Key]
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid StoreId { get; set; }
    [Required(ErrorMessage = "Quantity is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StocksOnHand { get; set; } = 0M;
    public int ReorderLevel { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    [Column(TypeName = "jsonb")]
    public virtual List<Stock> Stocks { get; set; } = new();
    public virtual List<ReturnedProduct> ReturnedProducts { get; set; } = new();
    [ForeignKey(nameof(StoreId))]
    public virtual Store? Store { get; set; }
    [ForeignKey(nameof(ItemId))]
    public virtual Item? Item { get; set; }
    public virtual ICollection<ProductOrderItem> OrderItems { get; set; } = new List<ProductOrderItem>();
}
