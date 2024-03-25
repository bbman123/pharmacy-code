using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;
using Shared.Models.Labs;
using Shared.Models.Orders;

namespace Shared.Models.Customers;

public class Customer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Customer Name field is required!")]
    public string? CustomerName { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone No must be atleast 11 digits")]
    public string? PhoneNo { get; set; }
    public Gender Gender { get; set; }
    [Required]
    public int? Age { get; set; }
    public string? ContactAddress { get; set; }
    public virtual List<LabOrder> LabOrders { get; set; } = new();
    public virtual List<PharmacyOrder> PharmacyOrders { get; set; } = new();
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
}