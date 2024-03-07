using Shared.Enums;
using Shared.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Users;

public class Referer
{
    [Required] public Guid Id { get; set; }
    [Required] public string? RefererName { get; set; }
    public RefererType Type { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public virtual ICollection<OrderReferer> OrderReferers { get; set; } = new List<OrderReferer>();
}
