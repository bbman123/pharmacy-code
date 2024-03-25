using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;

namespace Shared.Models.Orders;

public class Payment
{
    [Key]
    public Guid Id { get; set; }
    public DateTime? PaymentDate { get; set; }
    public PaymentMode PaymentMode { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }    
    public DateTime CreatedDate { get; set; }
}
