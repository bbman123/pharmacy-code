using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shared.Models.Products;

[Keyless]
public class Stock
{
    public DateTime Date { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Quantity is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }
}

