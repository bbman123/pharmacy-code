using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.Products;
public class ReturnedProduct
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Quantity is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Cost { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }
}