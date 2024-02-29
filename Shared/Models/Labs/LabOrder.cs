using Shared.Enums;
using Shared.Helpers;
using Shared.Models.Company;
using Shared.Models.Customers;
using Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Labs;

public class LabOrder
{
    [Required] public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly OrderDate { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Consultation { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total => Items!.Sum(x => x.Cost) +  Consultation;
    public TestStatus Status { get; set; }
    public PaymentMode PaymentMode { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
    public virtual ICollection<LabOrderItem>? Items { get; set; } = new List<LabOrderItem>();
    public virtual ICollection<LabDiagonse>? Diagonses { get; set; } = new List<LabDiagonse>();
    public virtual ICollection<OrderReferer> OrderReferers { get; set; } = new List<OrderReferer>();
    public virtual User? User { get; set; }
    public virtual Customer? Customer { get; set; }
    [ForeignKey(nameof(StoreId))]
    public virtual Store? Store { get; set; }
}
