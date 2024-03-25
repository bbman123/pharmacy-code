using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;
using Shared.Models.Labs;
using Shared.Models.Orders;

namespace Shared.Models.Customers;

public class Dealer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Dealer Name field is required!")]
    public string? DealerName { get; set; }
    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone No must be atleast 11 digits")]
    public string? PhoneNo { get; set; }
    public string? ContactAddress { get; set; }
    public List<PharmacyOrder> Orders { get; set; } = new();
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
}
