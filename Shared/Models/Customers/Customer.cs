using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models.Labs;

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
    public string? ContactAddress { get; set; }
    public List<LabOrder> Orders { get; set; } = new();
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
}