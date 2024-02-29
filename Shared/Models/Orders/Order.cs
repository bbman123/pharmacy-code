using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;
using System.Text.Json.Serialization;
using Shared.Models.Labs;
using Shared.Models.Users;
using Shared.Helpers;
using Shared.Models.Company;

namespace Shared.Models.Orders;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public int ReceiptNo { get; set; }
    public DateOnly OrderDate { get; set; }   
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Consultation { get; set; }    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount => ProductOrders.Sum(x => (decimal)x.Quantity * x.Cost) + Referers.Sum(x => x.Price) + Consultation;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SubTotal => TotalAmount;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    public virtual ICollection<ProductOrderItem> ProductOrders { get; set; } = new List<ProductOrderItem>();
    public virtual ICollection<OrderReferer> Referers { get; set; } = new List<OrderReferer>();
    [ForeignKey(nameof(StoreId))]
    public virtual Store? Store { get; set; }
}
