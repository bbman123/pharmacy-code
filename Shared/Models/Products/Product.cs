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
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StoreId { get; set; }
    public Guid CategoryID { get; set; }
    [Required(ErrorMessage = "Product Name is required")]
    public string? ProductName { get; set; }
    public string? Description { get; set; } = "";
    public string? Barcode { get; set; } = "";
    [Required(ErrorMessage = "Price is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0M;
    [Required(ErrorMessage = "Quantity is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StocksOnHand { get; set; } = 0M;
    public int ReorderLevel { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    [ForeignKey("CategoryID")]
    public virtual Category? Category { get; set; }
    [Column(TypeName = "jsonb")]
    public virtual List<Stock> Stocks { get; set; } = new();
    [ForeignKey(nameof(StoreId))]
    public virtual Store? Store { get; set; }
    public virtual ICollection<ProductOrderItem> OrderItems { get; set; } = new List<ProductOrderItem>();
}
