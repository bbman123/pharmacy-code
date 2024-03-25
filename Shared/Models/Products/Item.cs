using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Products;

public class Item
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();    
    public Guid CategoryID { get; set; }
    [Required(ErrorMessage = "Product Name is required")]
    public string? ProductName { get; set; }
    public string? Description { get; set; } = "";
    public string? Barcode { get; set; } = "";
    [Required(ErrorMessage = "Price is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0M;
    
    public DateOnly? ExpiryDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    [ForeignKey("CategoryID")]
    public virtual Category? Category { get; set; }    
}
