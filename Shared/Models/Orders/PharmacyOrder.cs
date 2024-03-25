using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;
using System.Text.Json.Serialization;
using Shared.Models.Labs;
using Shared.Models.Users;
using Shared.Models.Company;
using Shared.Models.Products;
using Shared.Models.Customers;

namespace Shared.Models.Orders;

public class PharmacyOrder
{
    [Key]
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid UserId { get; set; }
    public Guid StoreId { get; set; }
    public Guid? ConsultantId { get; set; }
    public int ReceiptNo { get; set; }
    public DateOnly OrderDate { get; set; }   
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Consultation { get; set; }  
    public string? ConsultationNote { get; set; }    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount => ProductOrders.Sum(x => x.Quantity * x.Cost) 
                                  - ReturnedProducts.Sum(x => x.Quantity!.Value * x.Cost!.Value)
                                  + Referers.Sum(x => x.Price)
                                  + Consultation;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SubTotal => TotalAmount - Discount;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Balance => SubTotal - Payments.Sum(p => p.Amount);
    public PaymentStatus GetPaymentStatus()
    {
        if (Balance == 0)
            return PaymentStatus.Full;
        else if (Balance < SubTotal)
            return PaymentStatus.Half;
        else
            return PaymentStatus.Unpaid;
        
    }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    public virtual ICollection<ProductOrderItem> ProductOrders { get; set; } = new List<ProductOrderItem>();
    public virtual ICollection<ReturnedProduct> ReturnedProducts { get; set; } = new List<ReturnedProduct>();
    public virtual ICollection<OrderReferer> Referers { get; set; } = new List<OrderReferer>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    [ForeignKey(nameof(StoreId))]
    public virtual Store? Store { get; set; }
    [ForeignKey(nameof(ConsultantId))]
    public virtual User? ConsultedBy { get; set; }
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }
}
