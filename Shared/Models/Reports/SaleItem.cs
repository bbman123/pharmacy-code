using Shared.Models.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace HyLook.Shared.Models;
public class SaleItem
{
    public Guid Id { get; set; }
    public Customer? Customer { get; set; }
    public int ProductReceiptNo { get; set; }
    public int ServiceReceiptNo { get; set; }
    public string? ReceiptNo => GetReceiptNo();
    public string? Item { get; set; }
    public string? Option { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public dynamic Quantity { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost => UnitPrice;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total => (decimal)(Quantity) * Cost;     
    public decimal GrandTotal { get; set; }

    private string? GetReceiptNo()
    {
        if (ProductReceiptNo > 0 && ServiceReceiptNo > 0)
            return string.Join(",", ServiceReceiptNo.ToString().PadLeft(4, '0'), ProductReceiptNo.ToString().PadLeft(4, '0'));
        else if (ProductReceiptNo > 0)
            return ProductReceiptNo.ToString().PadLeft(4, '0');
        else
            return ServiceReceiptNo.ToString().PadLeft(4, '0');
    }
}