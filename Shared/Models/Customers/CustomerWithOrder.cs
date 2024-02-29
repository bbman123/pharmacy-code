namespace Shared.Models.Customers;

#nullable disable
public class CustomerWithOrder
{
    public Guid OrderId { get; set; }
    public string ReceiptNo { get; set; }
    public DateTime OrderDate { get; set; }
    public string Customer { get; set; }
    public string PhoneNo { get; set; }
    public string ContactAddress { get; set; }
    public string OfficeAddress { get; set; }
    public string Email { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal SubTotal => TotalAmount - Discount;
    public decimal AmountPaid { get; set; }
    public decimal Balance => SubTotal - AmountPaid;
    public decimal Vat { get; set; }
    public string PaymentMode { get; set; }
    public bool MessageStatus { get; set; }
    //public virtual List<ReceiptItem> OrderDetails { get; set; } = new();
    //public virtual List<ProductOrderDetails> ProductOrderDetails { get; set; } = new();
}