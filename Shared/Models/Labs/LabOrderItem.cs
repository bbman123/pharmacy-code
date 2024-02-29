using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Labs;

public class LabOrderItem
{
    [Required]
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ServiceId { get; set; }
    public string? ServiceName { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost { get; set; }
    [ForeignKey(nameof(OrderId))]
    public virtual LabOrder? Order { get; set; }
    [ForeignKey(nameof(ServiceId))]
    public virtual LabTest? Service { get; set; }
}
